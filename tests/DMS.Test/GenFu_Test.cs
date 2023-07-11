using DMS.Test.Model;
using Google.Protobuf;
using NetTaste;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Xunit;

namespace DMS.Test
{
    public class GenFu_Test
    {
        [Fact]
        public void Radmon100()
        {
            var data = GenFu.GenFu.ListOf<UserEntity>(100);
        }
    }
}
