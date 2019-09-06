namespace Sitecore.Support.Shell.Applications.ContentEditor
{
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.Text;
  using Sitecore.Web.UI.HtmlControls;
  using System;
  using System.Linq;

  /// <summary>
  /// Represents a Tree field.
  /// </summary>
  #region Modified code
  public class Tree : Sitecore.Support.Web.UI.HtmlControls.TreePicker
  #endregion
  {
    private string _fieldname;

    private string _source;

    private string _itemid;

    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    /// <value>The name of the field.</value>
    public string FieldName
    {
      get
      {
        return StringUtil.GetString(this._fieldname);
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        this._fieldname = value;
      }
    }

    /// <summary>
    /// Gets or sets the item ID.
    /// </summary>
    /// <value>The item ID.</value>
    public string ItemID
    {
      get
      {
        return StringUtil.GetString(this._itemid);
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        this._itemid = value;
      }
    }

    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    /// <value>The source.</value>
    public string Source
    {
      get
      {
        return StringUtil.GetString(this._source);
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        if (!value.StartsWith("query:", StringComparison.InvariantCulture))
        {
          this._source = value;
        }
        else
        {
          Item item = Client.ContentDatabase.GetItem(this.ItemID);
          if (item != null)
          {
            #region Modified code: This fix is necessary only for SXA sites.
            if (value.StartsWith("query:$site", StringComparison.InvariantCulture))
            {
              // "{6669DC16-F106-44B5-96BE-7A31AE82B5B5}" is id of "/sitecore/templates/Foundation/Experience Accelerator/Multisite/Site"
              var list = item.Axes.GetAncestors();
              foreach (Item t in list)
              {
                var list2 = t.Template.BaseTemplates.ToList();
                foreach (TemplateItem t2 in list2)
                {
                  if (t2.ID.ToString() == "{6669DC16-F106-44B5-96BE-7A31AE82B5B5}")
                  {
                    this._source = t.Paths.FullPath;
                    return;
                  }
                }
              }
            }
            #endregion
            else
            {
              Item item2 = item.Axes.SelectSingleItem(value.Substring("query:".Length));
              if (item2 != null)
              {
                this._source = item2.ID.ToString();
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the item language.
    /// </summary>
    /// <value>The item language.</value>
    public string ItemLanguage
    {
      get
      {
        return base.GetViewStateString("ItemLanguage");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("ItemLanguage", value);
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Sitecore.Shell.Applications.ContentEditor.Tree" /> class.
    /// </summary>
    public Tree()
    {
      this._fieldname = string.Empty;
      this._source = string.Empty;
      this.Class = "scContentControl";
      base.Activation = true;
      base.AllowNone = true;
      base.SelectOnly = true;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      if (!Sitecore.Context.ClientPage.IsEvent)
      {
        DataContext dataContext = new DataContext();
        this.Controls.Add(dataContext);
        string text = string.Empty;
        string text2 = this.Source;
        if (text2.IndexOf("datasource=", StringComparison.InvariantCultureIgnoreCase) >= 0)
        {
          UrlString urlString = new UrlString(text2);
          text = urlString["databasename"];
          text2 = urlString["datasource"];
        }
        dataContext.ID = this.ID + "_datacontext";
        dataContext.DataViewName = "Master";
        dataContext.Root = text2;
        dataContext.Folder = this.Value;
        if (!string.IsNullOrEmpty(this.ItemLanguage))
        {
          dataContext.Language = Language.Parse(this.ItemLanguage);
        }
        if (!string.IsNullOrEmpty(text))
        {
          dataContext.Parameters = "databasename=" + text;
        }
        this.DataContext = this.ID + "_datacontext";
      }
      base.OnLoad(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      base.OnPreRender(e);
      base.ServerProperties["Value"] = base.ServerProperties["Value"];
    }

    /// <summary>
    /// Executes the changed event.
    /// </summary>
    protected override void DoChanged()
    {
      base.DoChanged();
      this.SetModified();
    }

    /// <summary>
    /// Sets the modified flag.
    /// </summary>
    protected override void SetModified()
    {
      base.SetModified();
      if (base.TrackModified)
      {
        Sitecore.Context.ClientPage.Modified = true;
      }
    }
  }
}