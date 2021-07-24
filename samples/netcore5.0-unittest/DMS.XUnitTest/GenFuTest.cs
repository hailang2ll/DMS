using DMS.XUnitTest.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.XUnitTest
{
    public class GenFuTest
    {
        public void Radmon100()
        {
            var data = GenFu.GenFu.ListOf<UserEntity>(100);
        }
    }
}
