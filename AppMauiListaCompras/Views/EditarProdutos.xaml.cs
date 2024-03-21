using AppMauiListaCompras.Models;
using System.Linq.Expressions;

namespace AppMauiListaCompras.Views;

public partial class EditarProdutos : ContentPage
{
    public EditarProdutos()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),

            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Produto Editado!", "OK");
            await Navigation.PopAsync();

        }
        catch (Exception ex)

        {
            await DisplayAlert("Ops", ex.Message, "Fechar");
        }
    }
}