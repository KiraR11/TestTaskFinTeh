namespace TestTaskFinTeh_Model
{
    public class Link
    {
        private readonly string _error_this_product_id_cannot_equal_up_product_id = "этот продукт и вышестоящий не могут быть одинаковыми";
        private readonly string _error_count_product_can_not_mune_one = "количесто продукции не может быть меньше 1";
        public Link(Product? upProduct, Product product, int count)
        {
            Product = product;
            UpProduct = upProduct;
            Count = count;
        }
        private Link() { }
        
        private Product? _upProduct;
        private Product _product;
        private int _count;
        public Product? UpProduct 
        {
            get => _upProduct;
            set 
            { 
                if(value != Product)
                    _upProduct = value;
                else 
                    throw new ArgumentException(_error_this_product_id_cannot_equal_up_product_id);
            } 
        }
        public Product Product
        {
            get => _product;
            set
            {
                if (value != UpProduct)
                    _product = value;
                else
                    throw new ArgumentException(_error_this_product_id_cannot_equal_up_product_id);
            }
        }
        public int Count 
        {
            get => _count;
            set
            {
                if(value < 1)
                    throw new ArgumentException(_error_count_product_can_not_mune_one);
                else
                    _count = value;
            }
        }
        public long Id { get; private set; }
    }
}
