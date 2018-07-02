using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Open.IO;

namespace Open.Facebook.Tests
{
    [TestClass]
    public class FacebookUnitTests
    {
        [TestMethod]
        public async Task UploadPhoto()
        {
            string accessToken = "";
            var client = new FacebookClient(accessToken);
            System.IO.Stream fileStream = null;
            var progressList = new List<StreamProgress>();
            var progress = new Progress<StreamProgress>(p =>
            {
                progressList.Add(p);
            });
            var photo = await client.UploadPhotoAsync("", "", fileStream, progress, CancellationToken.None);
            Assert.IsTrue(progressList.Count > 0);
        }
    }
}
