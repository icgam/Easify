using System;
using System.Collections.Generic;
using EasyApi.ExceptionHandling.ConfigurationBuilder;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.Providers;
using EasyApi.ExceptionHandling.UnitTests.Domain;
using NSubstitute;
using Xunit;

namespace EasyApi.ExceptionHandling.UnitTests.Formatter
{
    public class ErrorProviderTests
    {
        [Fact]
        public void ShouldReturnOriginalErrorMessage()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<OurApplicationException>()
            });

            var sut = new ErrorProvider();
            var exception = new OurApplicationException("Our App Error");

            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("Our App Error", result.Message);
            Assert.Equal("OurApplicationException", result.ErrorType);
            Assert.Empty(result.ChildErrors);
        }

        [Fact]
        public void ShouldReturnOriginalAndIgnoreInnerExceptionIfItsNotOfDesiredType()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<OurApplicationException>()
            });

            var sut = new ErrorProvider();
            var internalError = new Exception("Internal app error");
            var exception = new OurApplicationException("Our App Error", internalError);

            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("Our App Error", result.Message);
            Assert.Equal("OurApplicationException", result.ErrorType);
            Assert.Empty(result.ChildErrors);
        }

        [Fact]
        public void ShouldReturnGenericErrorInCaseNoApplicationHasBeenFoundWithinExceptionHierarchy()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.GenericError.Returns(new Error("Error", typeof(ApplicationExceptionBase).Name, new List<Error>()));
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<OurApplicationException>()
            });

            var sut = new ErrorProvider();
            var exception = new Exception("System Error");

            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("Error", result.Message);
            Assert.Equal("ApplicationExceptionBase", result.ErrorType);
            Assert.Empty(result.ChildErrors);
        }

        [Fact]
        public void ShouldReturnOriginalAndInnerExceptionAndIgnoreExceptionFiltering()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(true);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<OurApplicationException>()
            });

            var sut = new ErrorProvider();
            var internalError = new Exception("Internal app error");
            var exception = new OurApplicationException("Our App Error", internalError);
            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("Our App Error", result.Message);
            Assert.Equal("OurApplicationException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
               t1 =>
               {
                   Assert.Equal("Internal app error", t1.Message);
                   Assert.Equal("Exception", t1.ErrorType);
                   Assert.Empty(t1.ChildErrors);
               }
           );
        }

        [Fact]
        public void ShouldExtractErrorsFromAggregateException()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<ApplicationExceptionBase>()
            });
            
            var internalError1 = new OurApplicationException("Internal app error 1");
            var internalError2 = new AnotherOurApplicationException("Internal app error 2");
            var aggregateException = new AggregateException(new List<Exception>
            {
                internalError1,
                internalError2
            });
            var exception = new OurApplicationException("Our App Error", aggregateException);
            var sut = new ErrorProvider();

            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("Our App Error", result.Message);
            Assert.Equal("OurApplicationException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
               t1 =>
               {
                   Assert.Equal("Internal app error 1", t1.Message);
                   Assert.Equal("OurApplicationException", t1.ErrorType);
                   Assert.Empty(t1.ChildErrors);
               },
               t2 =>
               {
                   Assert.Equal("Internal app error 2", t2.Message);
                   Assert.Equal("AnotherOurApplicationException", t2.ErrorType);
                   Assert.Empty(t2.ChildErrors);
               }
           );
        }

        [Fact]
        public void ShouldExtractErrorsFromAggregateExceptionForGivenApplicationException()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<ApplicationExceptionBase>()
            });

            var internalError1 = new OurApplicationException("Internal app error 1");
            var internalError2 = new AnotherOurApplicationException("Internal app error 2");
            var internalError3 = new Exception("Internal app error 3");
            var aggregateException = new AggregateException(new List<Exception>
            {
                internalError1,
                internalError2,
                internalError3
            });
            var exception = new OurApplicationException("Our App Error", aggregateException);
            var sut = new ErrorProvider();

            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("Our App Error", result.Message);
            Assert.Equal("OurApplicationException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
               t1 =>
               {
                   Assert.Equal("Internal app error 1", t1.Message);
                   Assert.Equal("OurApplicationException", t1.ErrorType);
                   Assert.Empty(t1.ChildErrors);
               },
               t2 =>
               {
                   Assert.Equal("Internal app error 2", t2.Message);
                   Assert.Equal("AnotherOurApplicationException", t2.ErrorType);
                   Assert.Empty(t2.ChildErrors);
               }
           );
        }

        [Fact]
        public void ShouldExtractErrorsFromDeepExceptionHierarchies()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<ApplicationExceptionBase>()
            });

            var leafError1 = new OurApplicationException("(Leaf) Internal app error 1");
            var leafError2 = new AnotherOurApplicationException("(Leaf) Internal app error 2");
            var leafError3 = new Exception("(Leaf) Internal app error 3");
            var leafError4 = new AnotherOurApplicationException("(Leaf) Internal app error 4");

            var branchError1 = new OurApplicationException("(Branch) app error 1", leafError1);
            var branchError2 = new OurApplicationException("(Branch) app error 2", leafError2);

            var branchError3Aggregate = new AggregateException(new List<Exception>
            {
                branchError1,
                branchError2,
                leafError3,
                leafError4
            });

            var rootError = new AnotherOurApplicationException("(Root) app error", branchError3Aggregate);
            var sut = new ErrorProvider();

            // Act
            var result = sut.ExtractErrorsFor(rootError, options);

            // Assert
            Assert.Equal("(Root) app error", result.Message);
            Assert.Equal("AnotherOurApplicationException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
               t1 =>
               {
                   Assert.Equal("(Branch) app error 1", t1.Message);
                   Assert.Equal("OurApplicationException", t1.ErrorType);
                   Assert.Collection(t1.ChildErrors, l1 =>
                   {
                       Assert.Equal("(Leaf) Internal app error 1", l1.Message);
                       Assert.Equal("OurApplicationException", l1.ErrorType);
                       Assert.Empty(l1.ChildErrors);
                   });
               },
               t2 =>
               {
                   Assert.Equal("(Branch) app error 2", t2.Message);
                   Assert.Equal("OurApplicationException", t2.ErrorType);
                   Assert.Collection(t2.ChildErrors, l1 =>
                   {
                       Assert.Equal("(Leaf) Internal app error 2", l1.Message);
                       Assert.Equal("AnotherOurApplicationException", l1.ErrorType);
                       Assert.Empty(l1.ChildErrors);
                   });
               },
               t3 =>
               {
                   Assert.Equal("(Leaf) Internal app error 4", t3.Message);
                   Assert.Equal("AnotherOurApplicationException", t3.ErrorType);
                   Assert.Empty(t3.ChildErrors);
               }
           );
        }

        [Fact]
        public void ShouldReturnExceptionOfAddionalOrThirdPartyLibraryNestedExceptionTypes()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<ApplicationExceptionBase>(),
                new ExceptionRuleForErrorProvider<ThirdPartyFailureException>()
            });

            var internalThirdPartyLibError = new ThirdPartyFailureException("External library error");
            var internalError = new OurApplicationException("Internal app error 1", internalThirdPartyLibError);
            var exception = new OurApplicationException("Our App Error", internalError);

            var sut = new ErrorProvider();

            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("Our App Error", result.Message);
            Assert.Equal("OurApplicationException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
               t1 =>
               {
                   Assert.Equal("Internal app error 1", t1.Message);
                   Assert.Equal("OurApplicationException", t1.ErrorType);
                   Assert.Collection(t1.ChildErrors, l1 =>
                   {
                       Assert.Equal("External library error", l1.Message);
                       Assert.Equal("ThirdPartyFailureException", l1.ErrorType);
                       Assert.Empty(l1.ChildErrors);
                   });
               }
           );
        }

        [Fact]
        public void ShouldExtractDerivedExceptionMessages()
        {
            // Arrange
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<ApplicationExceptionBase>()
            });

            var internalError = new OurDerivedApplicationException("Derived Error");
            var exception = new Exception("Internal Error", internalError);

            var sut = new ErrorProvider();

            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("Derived Error", result.Message);
            Assert.Equal("OurDerivedApplicationException", result.ErrorType);
            Assert.Empty(result.ChildErrors);
        }

        [Fact]
        public void GivenExceptionWhenMatchingExceptionRuleSuppliedThenReturnCustomizedErrorMessage()
        {
            // Arrange
            var ruleMock = Substitute.For<IExceptionRule>();
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                ruleMock
            });

            ruleMock.CanHandle(Arg.Any<OurApplicationException>()).Returns(true);
            ruleMock.GetError(Arg.Any<OurApplicationException>(), Arg.Any<IEnumerable<Error>>(), false)
                .Returns(c =>
                {
                    var ex = c.Arg<OurApplicationException>();
                    var errors = c.Arg<IEnumerable<Error>>();
                    return new Error($"*** {ex.Message} ***", ex.GetType().Name, errors);
                });

            var sut = new ErrorProvider();
            var exception = new OurApplicationException("Our App Error");

            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("*** Our App Error ***", result.Message);
            Assert.Equal("OurApplicationException", result.ErrorType);
            Assert.Empty(result.ChildErrors);
        }

        [Fact]
        public void GivenExceptionWhenMatchingRuleSuppliedForInternalExceptionThenReturnCustomizedErrorMessage()
        {
            // Arrange
            var ruleMock = Substitute.For<IExceptionRule>();
            var options = Substitute.For<IErrorProviderOptions>();
            options.IncludeSystemLevelExceptions.Returns(false);
            options.ExceptionsToHandle.Returns(new List<IExceptionRule>
            {
                ruleMock
            });

            ruleMock.CanHandle(Arg.Any<ApplicationExceptionBase>()).Returns(true);
            ruleMock.GetError(Arg.Any<ApplicationExceptionBase>(), Arg.Any<IEnumerable<Error>>(), false)
                .Returns(c =>
                {
                    var ex = c.Arg<ApplicationExceptionBase>();
                    var errors = c.Arg<IEnumerable<Error>>();
                    return new Error($"*** {ex.Message} ***", ex.GetType().Name, errors);
                });

            var sut = new ErrorProvider();
            var internalError = new OurApplicationException("Internal app error");
            var exception = new AnotherOurApplicationException("Error", internalError);
            // Act
            var result = sut.ExtractErrorsFor(exception, options);

            // Assert
            Assert.Equal("*** Error ***", result.Message);
            Assert.Equal("AnotherOurApplicationException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
                t1 =>
                {
                    Assert.Equal("*** Internal app error ***", t1.Message);
                    Assert.Equal("OurApplicationException", t1.ErrorType);
                    Assert.Empty(t1.ChildErrors);
                }
            );
        }
    }
}