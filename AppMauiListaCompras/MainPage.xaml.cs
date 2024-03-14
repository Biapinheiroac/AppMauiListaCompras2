using System.Collections.ObjectModel;
using AppMauiListaCompras.Models;

namespace AppMauiListaCompras
{
    public partial class MainPage : ContentPage
    {
         ObservableCollection<Produto> lista_produto = new ObservableCollection<Produto>();

        public MainPage()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = lista_produto;
        }

        private void ToolbarItem_Clicked_somar(object sender, EventArgs e)
        {
            double soma = lista_produto.Sum(i => (i.Preco * i.Quantidade));
            string msg = $"O total é {soma:C}";
            DisplayAlert("Somatória", msg, "Fechar");

        }

        protected override void OnAppearing()
        {
           if(lista_produto.Count ==0) 
           {
                Task.Run(async () =>
                {
                    List<Produto> tmp = await App.Db.GetAll();
                    foreach (Produto p in tmp)
                    {
                        lista_produto.Add(p);
                    }
                });
           }
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;
            lista_produto.Clear();
            Task.Run(async () =>
            {
                List<Produto> tmp = await App.Db.Search(q);
                foreach (Produto p in tmp)
                {
                    lista_produto.Add(p);
                }
            });
        }

        private void ref_carregando_Refreshing(object sender, EventArgs e)
        {
            lista_produto.Clear();
            Task.Run(async () =>
            {
                List<Produto> tmp = await App.Db.GetAll();
                foreach (Produto p in tmp)
                {
                    lista_produto.Add(p);
                }
            });
            //ref.IsRefreshing = false;
        }

        private void ToolbarItem_Clicked_Add(object sender, EventArgs e)
        {

        }

        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
        

        
        private void MenuItem_Clicked_remover(object sender, EventArgs e)
        {

        }

        
    }

}
