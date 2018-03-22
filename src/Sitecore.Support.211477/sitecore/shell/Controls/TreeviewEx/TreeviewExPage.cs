namespace Sitecore.Support.Shell.Controls.TreeviewEx
{
  using Sitecore;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.Web;
  using Sitecore.Web.UI.WebControls;
  using System;
  using System.Web.UI;

  /// <summary>
  ///
  /// </summary>
  public class TreeviewExPage : Page
  {
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(e, "e");
      TreeviewEx treeviewEx = MainUtil.GetBool(WebUtil.GetQueryString("mr"), false) ? new MultiRootTreeview() : new TreeviewEx();
      this.Controls.Add(treeviewEx);
      treeviewEx.ID = WebUtil.GetQueryString("treeid");
      string queryString = WebUtil.GetQueryString("db", Client.ContentDatabase.Name);
      Database database = Factory.GetDatabase(queryString);
      Assert.IsNotNull(database, queryString);
      ID itemId = ShortID.DecodeID(WebUtil.GetQueryString("id"));
      string queryString2 = WebUtil.GetQueryString("la");
      Language language;
      if (string.IsNullOrEmpty(queryString2) || !Language.TryParse(queryString2, out language))
      {
        language = Sitecore.Context.Language;
      }
      Item item = database.GetItem(itemId, language);
      if (item != null)
      {
        treeviewEx.ParentItem = item;
      }
    }
  }
}