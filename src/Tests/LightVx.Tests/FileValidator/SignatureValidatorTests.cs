using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LightVx.Tests.Files;
using LightVx.Validators.File;

namespace LightVx.Tests.FileValidator
{
    [TestClass]
    public class SignatureValidatorTests
    {
        [TestMethod]
        public void JpgTest_Valid()
        {
            ValidateFileForSuccess(TestFileEnum.SmallJpg, new JpgSignatureValidator());
        }
        [TestMethod]
        public void JpgTest_InValid()
        {
            ValidateFileForFailure(TestFileEnum.SmallGif, new JpgSignatureValidator());
            ValidateFileForFailure(TestFileEnum.SmallPng, new JpgSignatureValidator());
            ValidateFileForFailure(TestFileEnum.SmallSvg, new JpgSignatureValidator());
        }

        [TestMethod]
        public void PngTest_Valid()
        {
            ValidateFileForSuccess(TestFileEnum.SmallPng, new PngSignatureValidator());
        }
        [TestMethod]
        public void PngTest_InValid()
        {
            ValidateFileForFailure(TestFileEnum.SmallGif, new PngSignatureValidator());
            ValidateFileForFailure(TestFileEnum.SmallJpg, new PngSignatureValidator());
            ValidateFileForFailure(TestFileEnum.SmallSvg, new PngSignatureValidator());
        }

        [TestMethod]
        public void GifTest_Valid()
        {
            ValidateFileForSuccess(TestFileEnum.SmallGif, new GifSignatureValidator());
        }
        [TestMethod]
        public void GifTest_InValid()
        {
            ValidateFileForFailure(TestFileEnum.SmallPng, new GifSignatureValidator());
            ValidateFileForFailure(TestFileEnum.SmallJpg, new GifSignatureValidator());
            ValidateFileForFailure(TestFileEnum.SmallSvg, new GifSignatureValidator());
        }

        private void ValidateFileForSuccess(TestFileEnum file, IValidator validator)
        {
            using (var stream = FileProvider.GetFile(file))
            {
                Assert.IsNotNull(stream);
                Assert.IsTrue(validator.Validate(stream));
            }
        }
        private void ValidateFileForFailure(TestFileEnum file, IValidator validator)
        {
            using (var stream = FileProvider.GetFile(file))
            {
                Assert.IsNotNull(stream);
                Assert.IsFalse(validator.Validate(stream));
            }
        }
    }
}
