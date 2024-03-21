using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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

        protected async override void OnAppearing()
        {
           if(lista_produto.Count ==0) 
           {
                    List<Produto> tmp = await App.Db.GetAll();
                    foreach (Produto p in tmp)
                    {
                        lista_produto.Add(p);
                    }
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

        private async void ToolbarItem_Clicked_Add(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.NovoProduto());
        }

        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Produto? p = e.SelectedItem as Produto;
            Navigation.PushAsync(new Views.EditarProdutos
            {
                BindingContext = p
            });
        }
        

        
        private async void MenuItem_Clicked_remover(object sender, EventArgs e)
        {
            try
            {
                MenuItem selecionado = (MenuItem)sender;

                Produto p = selecionado.BindingContext as Produto;

                bool confirm = await DisplayAlert(
                    "Tem certeza?", "Remover Produto?",
                    "Sim", "Cancelar");

                if (confirm)
                {
                    await App.Db.Delete(p.Id);
                    await DisplayAlert("Sucesso!",
                        "Produto Removido", "OK");
                }

            } catch { }
        }        
    }
}
