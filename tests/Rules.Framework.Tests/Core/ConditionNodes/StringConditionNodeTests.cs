namespace Rules.Framework.Tests.Core.ConditionNodes
{
    using FluentAssertions;
    using Rules.Framework.Core;
    using Rules.Framework.Core.ConditionNodes;
    using Rules.Framework.Tests.TestStubs;
    using Xunit;

    public class StringConditionNodeTests
    {
        [Fact]
        public void Init_GivenSetupWithStringValue_ReturnsSettedValues()
        {
            // Arrange
            ConditionType expectedConditionType = ConditionType.IsoCountryCode;
            Operators expectedOperator = Operators.NotEqual;
            string expectedOperand = "Such operand, much wow.";
            LogicalOperators expectedLogicalOperator = LogicalOperators.Eval;
            DataTypes expectedDataType = DataTypes.String;

            StringConditionNode<ConditionType> sut = new StringConditionNode<ConditionType>(expectedConditionType, expectedOperator, expectedOperand);

            // Act
            ConditionType actualConditionType = sut.ConditionType;
            Operators actualOperator = sut.Operator;
            DataTypes actualDataType = sut.DataType;
            LogicalOperators actualLogicalOperator = sut.LogicalOperator;
            string actualOperand = sut.Operand;

            // Assert
            actualConditionType.Should().Be(expectedConditionType);
            actualOperator.Should().Be(expectedOperator);
            actualOperand.Should().Be(expectedOperand);
            actualLogicalOperator.Should().Be(expectedLogicalOperator);
            actualDataType.Should().Be(expectedDataType);
        }
    }
}