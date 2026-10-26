namespace GestionArticles.Models.Help
{
    // Liste des produits sélectionnés (panier)
    public class ListeCart
    {
        public List<Item> Items { get; private set; } = new List<Item>();

        public static readonly ListeCart Instance;

        static ListeCart()
        {
            Instance = new ListeCart();
        }

        protected ListeCart() { }

        // Ajoute un produit au panier (ou augmente sa quantité s'il y est déjà)
        public void AddItem(Product prod)
        {
            foreach (Item a in Items)
            {
                if (a.Prod.ProductId == prod.ProductId)
                {
                    a.quantite++;
                    return;
                }
            }

            Item newItem = new Item(prod);
            newItem.quantite = 1;
            Items.Add(newItem);
        }

        // Vide le panier
        public void setToNUll()
        {
            Items.Clear();
        }

        // Retire une unité du produit (le supprime du panier s'il n'en reste plus)
        public void SetLessOneItem(Product prod)
        {
            foreach (Item a in Items)
            {
                if (a.Prod.ProductId == prod.ProductId)
                {
                    if (a.quantite <= 1)
                    {
                        RemoveItem(a.Prod);
                        return;
                    }
                    else
                    {
                        a.quantite--;
                        return;
                    }
                }
            }
        }

        public void SetItemQuantity(Product prod, int quantity)
        {
            if (quantity == 0)
            {
                RemoveItem(prod);
                return;
            }

            foreach (Item a in Items)
            {
                if (a.Prod.ProductId == prod.ProductId)
                {
                    a.quantite = quantity;
                    return;
                }
            }
        }

        public void RemoveItem(Product prod)
        {
            Item? t = null;
            foreach (Item a in Items)
            {
                if (a.Prod.ProductId == prod.ProductId)
                {
                    t = a;
                }
            }

            if (t != null)
            {
                Items.Remove(t);
            }
        }

        public decimal GetSubTotal()
        {
            decimal subTotal = 0;
            foreach (Item i in Items)
                subTotal += i.TotalPrice;
            return subTotal;
        }
    }
}
