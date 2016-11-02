namespace Repo2.AcceptanceTests.Lib
{
    public class ShouldIBuyMilk
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
