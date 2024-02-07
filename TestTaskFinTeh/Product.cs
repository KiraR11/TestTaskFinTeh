namespace TestTaskFinTeh_Model
{
    public class Product
    {
        private readonly string _error_name_empty = "Название не может быть пустым";
        private readonly string _error_name_cannot_longer_100_characters = "Название не может быть длинее 100 символов";
        private readonly string _error_fractional_part_price_cannot_store_more_2_characters = "Дробная часть цены не может хранить больше 2 знаков";
        private readonly string _error_whole_part_price_cannot_store_more_18_characters = "Целая часть цены не может хранить больше 18 знаков";
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        
        private decimal _price;
        private string _name;
        public long Id { get; private set; }
        public string Name { get => _name;
            set { if(CorrectName(value))_name = value; } }
        public decimal Price { get => _price;
            set {if(CorrectPrice(value)) _price = value; } }
        private bool CorrectName(string name)
        {
            if (name.Length == 0)
                throw new ArgumentException(_error_name_empty);
            if (name.Length > 100)
                throw new ArgumentException(_error_name_cannot_longer_100_characters);
            return true;
        }
        private bool CorrectPrice(decimal price)
        {
            if (NumberDigitsInFractionalPart(price) > 2)
                throw new ArgumentException(_error_fractional_part_price_cannot_store_more_2_characters);
            if (price.ToString().Length >= 18)
                throw new ArgumentException(_error_whole_part_price_cannot_store_more_18_characters);
            return true;
        }
        private int NumberDigitsInFractionalPart(decimal value)
        {
            int numderDigitsInFractionalPart = 0;
            decimal fractionalPart = value % 1;

            while (fractionalPart >= (decimal)Math.Pow(1, numderDigitsInFractionalPart * -1))
                numderDigitsInFractionalPart++;
            return numderDigitsInFractionalPart;
        }
        public override bool Equals(object? obj)
        {
            if (obj is Product)
            {
                var item = obj as Product;
                if (item == null && this == null)
                    return true;
                else if (item != null && item.Id.Equals(Id))
                    return true;
                else
                    return false;
            }
            else return false;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id.GetHashCode();
                return hashCode;
            }
        }
    }
}
