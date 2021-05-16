namespace Rules.Framework.Evaluation.ValueEvaluation.Dispatchers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rules.Framework.Core;

    internal class ConditionEvalDispatchProvider : IConditionEvalDispatchProvider
    {
        private const string ManyToMany = "many-to-many";
        private const string ManyToOne = "many-to-one";
        private const string OneToMany = "one-to-many";
        private const string OneToOne = "one-to-one";
        private readonly Dictionary<string, IConditionEvalDispatcher> dispatchers;
        private readonly string[] supportedCombinations;

        public ConditionEvalDispatchProvider(
            IOperatorEvalStrategyFactory operatorEvalStrategyFactory,
            IDataTypesConfigurationProvider dataTypesConfigurationProvider)
        {
            this.dispatchers = new Dictionary<string, IConditionEvalDispatcher>
            {
                { OneToOne, new OneToOneConditionEvalDispatcher(operatorEvalStrategyFactory, dataTypesConfigurationProvider) },
                { OneToMany, new OneToManyConditionEvalDispatcher(operatorEvalStrategyFactory, dataTypesConfigurationProvider) },
                { ManyToOne, new ManyToOneConditionEvalDispatcher(operatorEvalStrategyFactory, dataTypesConfigurationProvider) },
                { ManyToMany, new ManyToManyConditionEvalDispatcher(operatorEvalStrategyFactory, dataTypesConfigurationProvider) }
            };
            this.supportedCombinations = new[]
            {
                $"{OneToOne}-{Operators.Equal}",
                $"{OneToOne}-{Operators.NotEqual}",
                $"{OneToOne}-{Operators.GreaterThan}",
                $"{OneToOne}-{Operators.GreaterThanOrEqual}",
                $"{OneToOne}-{Operators.LesserThan}",
                $"{OneToOne}-{Operators.LesserThanOrEqual}",
                $"{OneToOne}-{Operators.Contains}",
            };
        }

        public IConditionEvalDispatcher GetEvalDispatcher(object leftOperand, Operators @operator, object rightOperand)
        {
            string combination = leftOperand switch
            {
                IComparable _ when rightOperand is IComparable => OneToOne,
                IComparable _ when rightOperand is IEnumerable<IComparable> => OneToMany,
                IEnumerable<IComparable> _ when rightOperand is IComparable => ManyToOne,
                IEnumerable<IComparable> _ when rightOperand is IEnumerable<IComparable> => ManyToMany,
                _ => throw new NotSupportedException()
            };

            this.ThrowIfUnsupportedOperandsAndOperatorCombination($"{combination}-{@operator}");

            return this.dispatchers[combination];
        }

        private void ThrowIfUnsupportedOperandsAndOperatorCombination(string combination)
        {
            if (!this.supportedCombinations.Contains(combination))
            {
                throw new NotSupportedException($"The combination '{combination}' is not supported.");
            }
        }
    }
}