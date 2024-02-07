namespace TestTaskFinTeh_Model
{
    public class LinkNode
    {
        public LinkNode(Link link,int countIncoming, decimal cost,List<LinkNode> downProducts)
        {
            Link = link;
            CountIncoming = countIncoming;
            Cost = cost;
            DownProducts = downProducts;
        }
        public Link Link { get;}
        public string Name => Link.Product.Name;
        public int Count => Link.Count;
        public decimal Price => Link.Product.Price;
        public decimal Cost { get; }
        public int CountIncoming { get;}
        public List<LinkNode> DownProducts { get;}
    }
}
