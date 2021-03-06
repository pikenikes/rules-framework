namespace Rules.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rules.Framework.Core;

    /// <summary>
    /// Exposes the interface contract for a rules data source for specified <typeparamref name="TContentType"/>.
    /// </summary>
    /// <typeparam name="TContentType">The content type that allows to categorize rules.</typeparam>
    /// <typeparam name="TConditionType">The condition type that allows to filter rules based on a set of conditions.</typeparam>
    public interface IRulesDataSource<TContentType, TConditionType>
    {
        /// <summary>
        /// Adds a new rule to data source.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        Task AddRuleAsync(Rule<TContentType, TConditionType> rule);

        /// <summary>
        /// Gets the rules categorized with specified <paramref name="contentType"/> between <paramref name="dateBegin"/> and <paramref name="dateEnd"/>.
        /// </summary>
        /// <param name="contentType">the content type categorization.</param>
        /// <param name="dateBegin">the filtering begin date.</param>
        /// <param name="dateEnd">the filtering end date.</param>
        /// <returns></returns>
        Task<IEnumerable<Rule<TContentType, TConditionType>>> GetRulesAsync(TContentType contentType, DateTime dateBegin, DateTime dateEnd);

        /// <summary>
        /// Gets the rules filtered by specified arguments.
        /// </summary>
        /// <param name="rulesFilterArgs">The rules filter arguments.</param>
        /// <returns></returns>
        Task<IEnumerable<Rule<TContentType, TConditionType>>> GetRulesByAsync(RulesFilterArgs<TContentType> rulesFilterArgs);

        /// <summary>
        /// Updates the existent rule on data source.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        Task UpdateRuleAsync(Rule<TContentType, TConditionType> rule);
    }
}