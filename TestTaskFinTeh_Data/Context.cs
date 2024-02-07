using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TestTaskFinTeh_Model;

namespace TestTaskFinTeh_Data
{
    public class Context : DbContext
    {
        public Context() 
        {
            try
            {
                if (!Database.CanConnect())
                {
                    Database.EnsureCreated();
                    GeneretData();
                }
            }
            catch ( ArgumentException ex )
            {
                throw new DbUpdateException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new DbUpdateException("Неверные данные строки подключения", ex);
            }
        }
        ~Context()
        {
            Database.CloseConnection();
        }
        private DbSet<Product> Products { get; set; }
        private DbSet<Link> Links { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = GetConnectionStrings();
            optionsBuilder.UseNpgsql(connectionString);
        }
        private string GetConnectionStrings()
        {
            string server = ConfigurationManager.ConnectionStrings["Server"].ConnectionString;
            string port = ConfigurationManager.ConnectionStrings["Port"].ConnectionString;
            string user = ConfigurationManager.ConnectionStrings["User"].ConnectionString;
            string password = ConfigurationManager.ConnectionStrings["Password"].ConnectionString;
            string database = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            return $@"Server={server};Port={port};Database={database};User Id={user};password={password};";
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>(entity => {
                
            });
            modelBuilder.Entity<Product>(entity => {

            });
        }
        public List<Product> GetAllProducts()
        {
            return Products.ToList();
        }
        public List<Link> GetAllLinks()
        {
            Products.ToList();
            return Links.ToList();

        }
        public List<Product> GetProductNotHaveLinks()
        {
            List <Product> products = Products.ToList();
            List<Product> productNotHaveLinks = new List<Product>();
            foreach (Product product in products)
            {
                if(!LinksConsictProbuct(product))
                    productNotHaveLinks.Add(product);
            }
            return productNotHaveLinks;
        }
        public List<Product> GetProductHaveLinks()
        {
            List<Product> products = Products.ToList();
            List<Product> productNotHaveLinks = new List<Product>();
            foreach (Product product in products)
            {
                if (LinksConsictProbuct(product))
                    productNotHaveLinks.Add(product);
            }
            return productNotHaveLinks;
        }
        private bool LinksConsictProbuct(Product product)
        {
            List<Link> links = Links.ToList();
            foreach (Link link in links)
            {
                if (link.Product == product)
                    return true;
            }
            return false;
        }
        public void AddProduct(Product product)
        {
            Product? productInDbWihtEqualName = Products.FirstOrDefault(c => c.Name.Equals(product.Name));
            if (productInDbWihtEqualName == null)
            {
                Add(product);
                SaveChanges();
            }
            else
                throw new DbUpdateException("Продукт с таким названием уже есть в базе данных");
        }
        public void AddLink(Link link)
        {
            Link? LinkInDb = Links.FirstOrDefault(c => c.Product == link.Product && c.UpProduct == link.UpProduct);
            if (LinkInDb == null)
            {
                Add(link);
                SaveChanges();
            }
            else
                throw new DbUpdateException("Ссылка с таким продуктом и вышестояшим продуктом уже есть в базе данных");
        }
        public void UpdateProduct(Product product)
        {
            Product? productInDbWihtEqualName = Products.FirstOrDefault(c => c.Name.Equals(product.Name));

            if(productInDbWihtEqualName != null && !productInDbWihtEqualName.Equals(product))
                throw new DbUpdateException("Продукт с таким названием уже есть в базе данных");
            else 
            {
                Update(product);
                SaveChanges();
            }
            
        }
        public void UpdateLink(Link link) 
        {
            Update(link);
            SaveChanges();
        }
        public void DeleteProduct(Product product)
        {
            Products.Remove(product);
            List<Link> df = Links.ToList(); 
            SaveChanges();
            
        }
        public void DeleteLink(Link link)
        {
            List<Link> links = Links.ToList();
            List<Link> IncomingLink = new List<Link>();
            foreach(Link thisLink in links)
            {
                if(thisLink.UpProduct == link.Product)
                    IncomingLink.Add(thisLink);
            }
            foreach(Link thisLink in IncomingLink)
            {
                thisLink.UpProduct = link.Product;
                UpdateLink(thisLink);
            }
            Links.Remove(link);
            SaveChanges();
        }
        
        private void GeneretData()
        {
            Product product1 = new Product("Изделие 1", 800);
            Product product2 = new Product("Изделие 2", 100);
            Product product3 = new Product("Изделие 3", 400);
            Product product4 = new Product("Изделие 4", 400);
            Product product5 = new Product("Изделие 5", 300);
            Product product6 = new Product("Изделие 6", 20);
            Product product7 = new Product("Изделие 7", 1000);
            Product product8 = new Product("Изделие 8", 100);

            Link link1 = new Link(null, product1, 1);
            Link link2 = new Link(null, product7, 1);
            Link link3 = new Link(product4, product2, 10);
            Link link4 = new Link(product1, product3, 2);
            Link link5 = new Link(product1, product4, 1);
            Link link6 = new Link(product7, product8, 20);
            Link link7 = new Link(product7, product5, 10);
            Link link8 = new Link(product4, product6, 5);

            AddLink(link1);
            AddLink(link2);
            AddLink(link3);
            AddLink(link4);
            AddLink(link5);
            AddLink(link6);
            AddLink(link7);
            AddLink(link8);

            SaveChanges();
        }

    }
}
