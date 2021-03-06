namespace Rules.Framework.Providers.InMemory.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Rules.Framework.Builder;
    using Rules.Framework.Providers.InMemory.Tests.TestStubs;
    using Xunit;

    public class InMemoryProviderRulesDataSourceSelectorExtensionsTests
    {
        [Fact]
        public void SetInMemoryDataSource_GivenNullRulesDataSourceSelector_ThrowsArgumentNullException()
        {
            // Arrange
            IRulesDataSourceSelector<ContentType, ConditionType> rulesDataSourceSelector = null;

            // Act
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => rulesDataSourceSelector.SetInMemoryDataSource());

            // Assert
            actual.Should().NotBeNull();
            actual.ParamName.Should().Be(nameof(rulesDataSourceSelector));
        }

        [Fact]
        public void SetInMemoryDataSource_GivenNullServiceProvider_ThrowsArgumentNullException()
        {
            // Arrange
            IServiceProvider serviceProvider = null;

            IRulesDataSourceSelector<ContentType, ConditionType> rulesDataSourceSelector = Mock.Of<IRulesDataSourceSelector<ContentType, ConditionType>>();

            // Act
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => rulesDataSourceSelector.SetInMemoryDataSource(serviceProvider));

            // Assert
            actual.Should().NotBeNull();
            actual.ParamName.Should().Be(nameof(serviceProvider));
        }

        [Fact]
        public void SetInMemoryDataSource_GivenServiceProvider_RequestsInMemoryRulesStorageAndSetsOnSelector()
        {
            // Arrange
            InMemoryRulesStorage<ContentType, ConditionType> inMemoryRulesStorage = Mock.Of<InMemoryRulesStorage<ContentType, ConditionType>>();

            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton(inMemoryRulesStorage);
            IServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();

            IRulesDataSourceSelector<ContentType, ConditionType> rulesDataSourceSelector = Mock.Of<IRulesDataSourceSelector<ContentType, ConditionType>>();

            IRulesDataSource<ContentType, ConditionType> actualRulesDataSource = null;
            Mock.Get(rulesDataSourceSelector)
                .Setup(x => x.SetDataSource(It.IsAny<IRulesDataSource<ContentType, ConditionType>>()))
                .Callback<IRulesDataSource<ContentType, ConditionType>>((rds) =>
                {
                    actualRulesDataSource = rds;
                });

            // Act
            rulesDataSourceSelector.SetInMemoryDataSource(serviceProvider);

            // Assert
            actualRulesDataSource.Should().NotBeNull();
            actualRulesDataSource.Should().BeOfType<InMemoryProviderRulesDataSource<ContentType, ConditionType>>();
            Mock.Get(rulesDataSourceSelector)
                .Verify();
        }

        [Fact]
        public void SetInMemoryDataSource_NoParametersGiven_CreatesTransientInMemoryRulesStorageAndSetsOnSelector()
        {
            // Arrange
            IRulesDataSourceSelector<ContentType, ConditionType> rulesDataSourceSelector = Mock.Of<IRulesDataSourceSelector<ContentType, ConditionType>>();

            IRulesDataSource<ContentType, ConditionType> actualRulesDataSource = null;
            Mock.Get(rulesDataSourceSelector)
                .Setup(x => x.SetDataSource(It.IsAny<IRulesDataSource<ContentType, ConditionType>>()))
                .Callback<IRulesDataSource<ContentType, ConditionType>>((rds) =>
                {
                    actualRulesDataSource = rds;
                });

            // Act
            rulesDataSourceSelector.SetInMemoryDataSource();

            // Assert
            actualRulesDataSource.Should().NotBeNull();
            actualRulesDataSource.Should().BeOfType<InMemoryProviderRulesDataSource<ContentType, ConditionType>>();
            Mock.Get(rulesDataSourceSelector)
                .Verify();
        }
    }
}