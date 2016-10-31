using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo2.AcceptanceTests.Lib
{
    public class ShouldIBuyMilk //: fitSharp.Fit.Fixtures.
    {
        private int _cash;
        private int _pintsOfMilkRemaining;
        private string _useCreditCard;

        public void SetCashInWallet(int cash)
        {
            _cash = cash;
        }

        public void SetCreditCard(string useCreditCard)
        {
            _useCreditCard = useCreditCard;
        }

        public void SetPintsOfMilkRemaining(int pints)
        {
            _pintsOfMilkRemaining = pints;
        }

        public string GoToStore()
        {
            if (_cash > 0 || _useCreditCard.Equals("yes"))
                return "yes";
            return "no";
        }
    }
}
