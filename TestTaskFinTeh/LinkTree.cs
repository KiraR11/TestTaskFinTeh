namespace TestTaskFinTeh_Model
{
    public class LinkTree
    {
        public LinkTree(List<Link> links, Product parentProduct) 
        {
            Links = links;
            RootProduct = GenerateProductTree(parentProduct,0);
        }
        public LinkTree(List<Link> links)
        {
            Links = links;
            RootProduct = GenerateProductTree(null,0);
        }
        public List<LinkNode> RootProduct { get; }
        private List<Link> Links { get; }
        public int _maxLevelHierarchy = 0;
        public int MaxLevelHierarchy { get => _maxLevelHierarchy;
            set { if (value > _maxLevelHierarchy) _maxLevelHierarchy = value; } }

        private List<LinkNode> GenerateProductTree(Product? parentProduct, int levelHierarchy)
        {
            List<LinkNode> productHierarchies = new List<LinkNode>();
            List<Link> linksWithThisParentProduct = FindlinksWithThisParentProduct(Links, parentProduct);

            foreach (Link link in linksWithThisParentProduct)
            {
                List<LinkNode> incomingProducts = GenerateProductTree(link.Product, levelHierarchy++);
                
                int countAllIncomingProduct = CalcCountIncomingProgucts(incomingProducts);
                decimal costProducts = link.Count * link.Product.Price + CalcCostProgucts(incomingProducts);

                Link linkTest = link;
                LinkNode productNode = new LinkNode(linkTest, countAllIncomingProduct, costProducts,incomingProducts);
                productHierarchies.Add(productNode);
            }
            MaxLevelHierarchy = levelHierarchy;
            return productHierarchies;
        }
        private List<Link> FindlinksWithThisParentProduct(List<Link> links,Product? thisProduct)
        {
            List<Link> linksWithThisParentProduct = new List<Link>();
            foreach (Link link in links)
            {
                if((link.UpProduct == null && thisProduct == null) || (link.UpProduct != null && link.UpProduct.Equals(thisProduct)))
                {
                    Link linkTest = link;
                    linksWithThisParentProduct.Add(linkTest);
                }
            }
            return linksWithThisParentProduct;  
        }
        private int CalcCountIncomingProgucts(List<LinkNode> productNodes)
        {
            int count = 0;

            foreach (LinkNode productNode in productNodes)
                count+= productNode.CountIncoming + productNode.Link.Count;

            return count;
        }
        private decimal CalcCostProgucts(List<LinkNode> productNodes)
        {
            decimal cost = 0;

            foreach (LinkNode productNode in productNodes)
                cost += productNode.Cost;

            return cost;
        }
    }
}
