using DMS.Test.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Test
{
    public class GenFu_Test
    {
        public void Radmon100()
        {
            var data = GenFu.GenFu.ListOf<UserEntity>(100);
        }
    }
}
