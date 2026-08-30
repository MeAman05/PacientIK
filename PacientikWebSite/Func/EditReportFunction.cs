namespace PacientikWebSite.Func
{
    public class EditReportFunction
    {
        SelectedTableColumn[] columns;
        public async Task EditColumn(string data, int id)
        {
            var product = columns.FirstOrDefault(c => c.Id == id);

            product.Name = data;
        }
    }
}
