using System;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace ArmaForces.Boderator.Core.Tests.TestUtilities
{
    public static class ResultAssertionsExtensions
    {
        public static void ShouldBeFailure(this Result result)
        {
            using var scope = new AssertionScope();

            if (result.IsSuccess)
            {
                result.IsSuccess.Should().BeFalse();
            }
        }

        public static void ShouldBeFailure<T>(this Result<T> result)
        {
            using var scope = new AssertionScope();

            if (result.IsSuccess)
            {
                result.IsSuccess.Should().BeFalse();
                result.Value.Should().BeNull();
            }
        }
        
        public static void ShouldBeFailure(this Result result, string expectedError)
        {
            using var scope = new AssertionScope();

            if (result.IsSuccess)
            {
                result.IsSuccess.Should().BeFalse();
            }
            else
            {
                result.Error.Should().Be(expectedError);
            }
        }

        public static void ShouldBeFailure<T>(this Result<T> result, string expectedError)
        {
            using var scope = new AssertionScope();

            if (result.IsSuccess)
            {
                result.IsSuccess.Should().BeFalse();
            }
            else
            {
                result.Error.Should().Be(expectedError);
            }
        }

        public static void ShouldBeSuccess(this Result result)
        {
            using var scope = new AssertionScope();

            if (result.IsFailure)
            {
                result.IsSuccess.Should().BeTrue();
                result.Error.Should().BeNull();
            }
        }

        public static void ShouldBeSuccess<T>(this Result<T> result)
        {
            using var scope = new AssertionScope();

            if (result.IsFailure)
            {
                result.IsSuccess.Should().BeTrue();
                result.Error.Should().BeNull();
            }

            result.Value.Should().NotBeNull();
        }

        public static void ShouldBeSuccess<T1, T2>(this Result<T1> result, T2 expectedValue)
        {
            using var scope = new AssertionScope();

            if (result.IsSuccess)
            {
                result.Value.Should().BeEquivalentTo(expectedValue);
            }
            else
            {
                result.IsSuccess.Should().BeTrue();
                result.Error.Should().BeNull();
            }
        }
        
        public static void ShouldBeSuccess<T1, T2>(
            this Result<T1> result,
            T2 expectedValue,
            Func<EquivalencyAssertionOptions<T2>,EquivalencyAssertionOptions<T2>> config)
        {
            using var scope = new AssertionScope();

            if (result.IsSuccess)
            {
                result.Value.Should().BeEquivalentTo(expectedValue, config);
            }
            else
            {
                result.IsSuccess.Should().BeTrue();
                result.Error.Should().BeNull();
            }
        }
        
        public static void ShouldBeSuccess<T1>(this Result<T1> result, Action<T1> valueAssertion)
        {
            using var scope = new AssertionScope();

            if (result.IsSuccess)
            {
                valueAssertion(result.Value);
            }
            else
            {
                result.IsSuccess.Should().BeTrue();
                result.Error.Should().BeNull();
            }
        }
    }
}
