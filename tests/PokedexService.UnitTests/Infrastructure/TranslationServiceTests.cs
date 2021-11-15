using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hosts.Domain.FunTranslations;
using Hosts.Infrastructure;
using Moq;
using NUnit.Framework;

namespace PokedexService.UnitTests.Infrastructure
{
    [TestFixture]
    public class TranslationServiceTests
    {
        private Mock<IFunTranslationsHttpClient> _funTranslationMock;
        private TranslationService _translationService;

        [SetUp]
        public void SetUp()
        {
            _funTranslationMock = new Mock<IFunTranslationsHttpClient>();
            _translationService = new TranslationService(_funTranslationMock.Object);
        }

        [Test]
        public async Task Translate()
        {
            var expectedTranslation = "Yoda here";
            _funTranslationMock.Setup(x => x.Translate(It.IsAny<string>(),It.IsAny<string>()))
                .ReturnsAsync(expectedTranslation);

            var result = await _translationService.Translate("original text", "Yoda");

            result.Should().Be(expectedTranslation);
         }

        [Test]
        public async Task Translate_defaults_to_original_when_translation_fails()
        {
            _funTranslationMock.Setup(x => x.Translate(It.IsAny<string>(),It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var result = await _translationService.Translate("original text", "Yoda");

            result.Should().Be("original text");
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task Translate_defaults_to_original_when_translation_is_empty(string translation)
        {
            _funTranslationMock.Setup(x => x.Translate(It.IsAny<string>(),It.IsAny<string>()))
                .ReturnsAsync(translation);

            var result = await _translationService.Translate("original text", "Yoda");

            result.Should().Be("original text");
        }
    }
}
