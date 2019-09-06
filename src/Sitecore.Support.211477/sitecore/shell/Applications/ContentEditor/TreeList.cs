namespace Sitecore.Support.Shell.Applications.ContentEditor
{
  using Sitecore;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.Resources;
  using Sitecore.Shell.Applications.ContentEditor;
  using Sitecore.Shell.Applications.ContentEditor.FieldHelpers;
  using Sitecore.Text;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.HtmlControls.Data;
  using Sitecore.Web.UI.Sheer;
  using Sitecore.Web.UI.WebControls;
  using System;
  using System.Collections;
  using System.ComponentModel;
  using System.Linq;
  using System.Web.UI;

  /// <summary>
  ///   Summary description for TreeMultiList.  
  /// </summary>
  public class TreeList : Sitecore.Web.UI.HtmlControls.Control, IContentField
  {
    private string _itemID;

    private Listbox _listBox;

    private string _source;

    /// <summary>
    /// The filter query builder that limits the number of elements to show.
    /// </summary>
    protected readonly Sitecore.Support.Shell.Applications.ContentEditor.FieldHelpers.TreeListFilterQueryBuilder FilterQueryBuilder;

    /// <summary>
    ///   Gets or sets a value indicating whether the <see cref="T:Sitecore.Shell.Applications.ContentEditor.TreeList" /> allows the multiple selection.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the <see cref="T:Sitecore.Shell.Applications.ContentEditor.TreeList" /> allows the  multiple selection; otherwise, <c>false</c>.
    /// </value>
    [Category("Data")]
    [Description("If set to Yes, allows the same item to be selected more than once")]
    public bool AllowMultipleSelection
    {
      get
      {
        return base.GetViewStateBool("AllowMultipleSelection");
      }
      set
      {
        base.SetViewStateBool("AllowMultipleSelection", value);
      }
    }

    /// <summary>
    ///   Gets the data context parameters.
    /// </summary>
    /// <value>The data context parameters.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="not null" />
    /// </contract>
    public string DatabaseName
    {
      get
      {
        return base.GetViewStateString("DatabaseName");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("DatabaseName", value);
      }
    }

    /// <summary>
    ///   Gets or sets field that will be used as source for ListItem header. If empty- DisplayName will be used.
    /// </summary>
    public string DisplayFieldName
    {
      get
      {
        return base.GetViewStateString("DisplayFieldName");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("DisplayFieldName", value);
      }
    }

    /// <summary>
    ///   Gets or sets the exclude items for display.
    /// </summary>
    /// <value>The exclude items for display.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="not null" />
    /// </contract>
    [Category("Data")]
    [Description("Comma separated list of item names/ids.")]
    public string ExcludeItemsForDisplay
    {
      get
      {
        return base.GetViewStateString("ExcludeItemsForDisplay");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("ExcludeItemsForDisplay", value);
      }
    }

    /// <summary>
    ///   Gets or sets the exclude templates for display.
    /// </summary>
    /// <value>The exclude templates for display.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="not null" />
    /// </contract>
    [Category("Data")]
    [Description("Comma separated list of template names. If this value is set, items based on these template will not be displayed in the tree.")]
    public string ExcludeTemplatesForDisplay
    {
      get
      {
        return base.GetViewStateString("ExcludeTemplatesForDisplay");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("ExcludeTemplatesForDisplay", value);
      }
    }

    /// <summary>
    ///   Gets or sets the exclude templates for selection.
    /// </summary>
    /// <value>The exclude templates for selection.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="not null" />
    /// </contract>
    [Category("Data")]
    [Description("Comma separated list of template names. If this value is set, items based on these template will not be included in the menu.")]
    public string ExcludeTemplatesForSelection
    {
      get
      {
        return base.GetViewStateString("ExcludeTemplatesForSelection");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("ExcludeTemplatesForSelection", value);
      }
    }

    /// <summary>
    ///   Gets or sets the include items for display.
    /// </summary>
    /// <value>The include items for display.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="not null" />
    /// </contract>
    [Category("Data")]
    [Description("Comma separated list of items names/ids.")]
    public string IncludeItemsForDisplay
    {
      get
      {
        return base.GetViewStateString("IncludeItemsForDisplay");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("IncludeItemsForDisplay", value);
      }
    }

    /// <summary>
    ///   Gets or sets the include templates for display.
    /// </summary>
    /// <value>The include templates for display.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="not null" />
    /// </contract>
    [Category("Data")]
    [Description("Comma separated list of template names. If this value is set, only items based on these template can be displayed in the menu.")]
    public string IncludeTemplatesForDisplay
    {
      get
      {
        return base.GetViewStateString("IncludeTemplatesForDisplay");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("IncludeTemplatesForDisplay", value);
      }
    }

    /// <summary>
    ///   Gets or sets the include templates for selection.
    /// </summary>
    /// <value>The include templates for selection.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="not null" />
    /// </contract>
    [Category("Data")]
    [Description("Comma separated list of template names. If this value is set, only items based on these template can be included in the menu.")]
    public string IncludeTemplatesForSelection
    {
      get
      {
        return base.GetViewStateString("IncludeTemplatesForSelection");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.SetViewStateString("IncludeTemplatesForSelection", value);
      }
    }

    /// <summary>
    ///   Gets or sets the item ID.
    /// </summary>
    /// <value>The item ID.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="nullable" />
    /// </contract>
    public string ItemID
    {
      get
      {
        return this._itemID;
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        this._itemID = value;
      }
    }

    /// <summary>
    ///   Gets or sets the item language.
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
    ///   Gets or sets a value indicating whether the <see cref="T:Sitecore.Shell.Applications.ContentEditor.TreeList" /> is read-only.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the <see cref="T:Sitecore.Shell.Applications.ContentEditor.TreeList" /> is read-only; otherwise, <c>false</c>.
    /// </value>
    public bool ReadOnly
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the source.
    /// </summary>
    /// <value>The source.</value>
    /// <contract>
    ///   <requires name="value" condition="not null" />
    ///   <ensures condition="nullable" />
    /// </contract>
    public string Source
    {
      get
      {
        return this._source;
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        this._source = value;
      }
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Sitecore.Shell.Applications.ContentEditor.TreeList" /> class.
    /// </summary>
    public TreeList()
    {
      this.Class = "scContentControl scContentControlTreelist";
      base.Activation = true;
      this.ReadOnly = false;
      this.FilterQueryBuilder = new Sitecore.Support.Shell.Applications.ContentEditor.FieldHelpers.TreeListFilterQueryBuilder();
    }

    /// <summary>
    ///   Raises the load event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <contract>
    ///   <requires name="args" condition="not null" />
    /// </contract>
    protected override void OnLoad(EventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      if (!Sitecore.Context.ClientPage.IsEvent)
      {
        this.SetProperties();
        Border border = new Border();
        this.Controls.Add(border);
        this.GetControlAttributes();
        foreach (string key in base.Attributes.Keys)
        {
          border.Attributes.Add(key, base.Attributes[key]);
        }
        border.Attributes["id"] = this.ID;
        Border border2 = new Border
        {
          Class = "scTreeListHalfPart"
        };
        border.Controls.Add(border2);
        Border border3 = new Border();
        border2.Controls.Add(border3);
        base.SetViewStateString("ID", this.ID);
        border3.Controls.Add(new Literal("All")
        {
          Class = "scContentControlMultilistCaption"
        });
        Scrollbox scrollbox = new Scrollbox
        {
          ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("S"),
          Class = "scScrollbox scContentControlTree"
        };
        border3.Controls.Add(scrollbox);
        #region Modified code
        Sitecore.Support.Web.UI.WebControls.TreeviewEx treeviewEx = new Sitecore.Support.Web.UI.WebControls.TreeviewEx
        #endregion
        {
          ID = this.ID + "_all",
          DblClick = this.ID + ".Add",
          AllowDragging = false
        };
        scrollbox.Controls.Add(treeviewEx);
        border3 = new Border
        {
          Class = "scContentControlNavigation"
        };
        border2.Controls.Add(border3);
        ImageBuilder imageBuilder = new ImageBuilder
        {
          Src = "Office/16x16/navigate_right.png",
          Class = "scNavButton",
          ID = this.ID + "_right",
          OnClick = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".Add")
        };
        ImageBuilder arg = new ImageBuilder
        {
          Src = "Office/16x16/navigate_left.png",
          Class = "scNavButton",
          ID = this.ID + "_left",
          OnClick = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".Remove")
        };
        LiteralControl child = new LiteralControl(imageBuilder.ToString() + arg);
        border3.Controls.Add(child);
        Border border4 = new Border
        {
          Class = "scTreeListHalfPart"
        };
        border.Controls.Add(border4);
        border3 = new Border
        {
          Class = "scFlexColumnContainerWithoutFlexie"
        };
        border4.Controls.Add(border3);
        border3.Controls.Add(new Literal("Selected")
        {
          Class = "scContentControlMultilistCaption"
        });
        Border border5 = new Border
        {
          Class = "scContentControlSelectedList"
        };
        border3.Controls.Add(border5);
        Listbox listbox = new Listbox();
        border5.Controls.Add(listbox);
        this._listBox = listbox;
        listbox.ID = this.ID + "_selected";
        listbox.DblClick = this.ID + ".Remove";
        listbox.Style["width"] = "100%";
        listbox.Size = "10";
        listbox.Attributes["onchange"] = "javascript:document.getElementById('" + this.ID + "_help').innerHTML=this.selectedIndex>=0?this.options[this.selectedIndex].innerHTML:''";
        listbox.Attributes["class"] = "scContentControlMultilistBox scFlexContentWithoutFlexie";
        this._listBox.TrackModified = false;
        treeviewEx.Enabled = !this.ReadOnly;
        listbox.Disabled = this.ReadOnly;
        border3.Controls.Add(new LiteralControl("<div class='scContentControlTreeListHelp' id=\"" + this.ID + "_help\"></div>"));
        border3 = new Border
        {
          Class = "scContentControlNavigation"
        };
        border4.Controls.Add(border3);
        ImageBuilder imageBuilder2 = new ImageBuilder
        {
          Src = "Office/16x16/navigate_up.png",
          Class = "scNavButton",
          ID = this.ID + "_up",
          OnClick = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".Up")
        };
        ImageBuilder arg2 = new ImageBuilder
        {
          Src = "Office/16x16/navigate_down.png",
          Class = "scNavButton",
          ID = this.ID + "_down",
          OnClick = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".Down")
        };
        child = new LiteralControl(imageBuilder2.ToString() + arg2);
        border3.Controls.Add(child);
        DataContext dataContext = new DataContext();
        border.Controls.Add(dataContext);
        dataContext.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("D");
        dataContext.Filter = this.FormTemplateFilterForDisplay();
        treeviewEx.DataContext = dataContext.ID;
        treeviewEx.DisplayFieldName = this.DisplayFieldName;
        dataContext.DataViewName = "Master";
        if (!string.IsNullOrEmpty(this.DatabaseName))
        {
          dataContext.Parameters = "databasename=" + this.DatabaseName;
        }
        dataContext.Root = this.DataSource;
        dataContext.Language = Language.Parse(this.ItemLanguage);
        treeviewEx.ShowRoot = true;
        this.RestoreState();
      }
      base.OnLoad(args);
    }

    /// <summary>
    ///   Adds data.
    /// </summary>
    protected void Add()
    {
      if (!this.Disabled)
      {
        string viewStateString = base.GetViewStateString("ID");
        TreeviewEx treeviewEx = this.FindControl(viewStateString + "_all") as TreeviewEx;
        Assert.IsNotNull(treeviewEx, typeof(DataTreeview));
        Listbox listbox = this.FindControl(viewStateString + "_selected") as Listbox;
        Assert.IsNotNull(listbox, typeof(Listbox));
        Item selectionItem = treeviewEx.GetSelectionItem(Language.Parse(this.ItemLanguage), Sitecore.Data.Version.Latest);
        if (selectionItem == null)
        {
          SheerResponse.Alert("Select an item in the Content Tree.");
        }
        else if (!this.HasExcludeTemplateForSelection(selectionItem))
        {
          if (this.IsDeniedMultipleSelection(selectionItem, listbox))
          {
            SheerResponse.Alert("You cannot select the same item twice.");
          }
          else if (this.HasIncludeTemplateForSelection(selectionItem))
          {
            SheerResponse.Eval("scForm.browser.getControl('" + viewStateString + "_selected').selectedIndex=-1");
            ListItem listItem = new ListItem();
            listItem.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("L");
            Sitecore.Context.ClientPage.AddControl(listbox, listItem);
            listItem.Header = this.GetHeaderValue(selectionItem);
            listItem.Value = listItem.ID + "|" + selectionItem.ID;
            SheerResponse.Refresh(listbox);
            TreeList.SetModified();
          }
        }
      }
    }

    /// <summary>
    ///   Executes the Down event.
    /// </summary>
    protected void Down()
    {
      if (!this.Disabled)
      {
        string viewStateString = base.GetViewStateString("ID");
        Listbox listbox = this.FindControl(viewStateString + "_selected") as Listbox;
        Assert.IsNotNull(listbox, typeof(Listbox));
        int num = -1;
        for (int num2 = listbox.Controls.Count - 1; num2 >= 0; num2--)
        {
          ListItem listItem = listbox.Controls[num2] as ListItem;
          Assert.IsNotNull(listItem, typeof(ListItem));
          if (!listItem.Selected)
          {
            num = num2 - 1;
            break;
          }
        }
        for (int num3 = num; num3 >= 0; num3--)
        {
          ListItem listItem = listbox.Controls[num3] as ListItem;
          Assert.IsNotNull(listItem, typeof(ListItem));
          if (listItem.Selected)
          {
            string[] values = new string[5]
            {
                        "scForm.browser.swapNode(scForm.browser.getControl('",
                        listItem.ID,
                        "'), scForm.browser.getControl('",
                        listItem.ID,
                        "').nextSibling);"
            };
            SheerResponse.Eval(string.Concat(values));
            listbox.Controls.Remove(listItem);
            listbox.Controls.AddAt(num3 + 1, listItem);
          }
        }
        TreeList.SetModified();
      }
    }

    /// <summary>
    ///   Gets the header value.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>Header text for list item.</returns>
    protected virtual string GetHeaderValue(Item item)
    {
      Assert.ArgumentNotNull(item, "item");
      #region Modified code
      string text = string.IsNullOrEmpty(this.DisplayFieldName) ? ItemExtension.GetUIDisplayName(item) : ((BaseItem)item)[this.DisplayFieldName];
      #endregion
      return string.IsNullOrEmpty(text) ? item.DisplayName : text;
    }

    /// <summary>
    ///   Removes the selected item.
    /// </summary>
    protected void Remove()
    {
      if (!this.Disabled)
      {
        string viewStateString = base.GetViewStateString("ID");
        Listbox listbox = this.FindControl(viewStateString + "_selected") as Listbox;
        Assert.IsNotNull(listbox, typeof(Listbox));
        SheerResponse.Eval("scForm.browser.getControl('" + viewStateString + "_all').selectedIndex=-1");
        SheerResponse.Eval("scForm.browser.getControl('" + viewStateString + "_help').innerHTML=''");
        ListItem[] selected = listbox.Selected;
        foreach (ListItem listItem in selected)
        {
          SheerResponse.Remove(listItem.ID);
          listbox.Controls.Remove(listItem);
        }
        SheerResponse.Refresh(listbox);
        TreeList.SetModified();
      }
    }

    /// <summary>
    ///   Moves the selected items up.
    /// </summary>
    protected void Up()
    {
      if (!this.Disabled)
      {
        string viewStateString = base.GetViewStateString("ID");
        Listbox listbox = this.FindControl(viewStateString + "_selected") as Listbox;
        Assert.IsNotNull(listbox, typeof(Listbox));
        ListItem selectedItem = listbox.SelectedItem;
        if (selectedItem != null)
        {
          int num = listbox.Controls.IndexOf(selectedItem);
          if (num != 0)
          {
            string[] values = new string[5]
            {
                        "scForm.browser.swapNode(scForm.browser.getControl('",
                        selectedItem.ID,
                        "'), scForm.browser.getControl('",
                        selectedItem.ID,
                        "').previousSibling);"
            };
            SheerResponse.Eval(string.Concat(values));
            listbox.Controls.Remove(selectedItem);
            listbox.Controls.AddAt(num - 1, selectedItem);
            TreeList.SetModified();
          }
        }
      }
    }

    /// <summary>
    ///   Gets the value.
    /// </summary>
    /// <returns>The value of the field.</returns>
    /// <contract>
    ///   <ensures condition="not null" />
    /// </contract>
    public string GetValue()
    {
      ListString listString = new ListString();
      string viewStateString = base.GetViewStateString("ID");
      Listbox listbox = this.FindControl(viewStateString + "_selected") as Listbox;
      Assert.IsNotNull(listbox, typeof(Listbox));
      ListItem[] items = listbox.Items;
      foreach (ListItem listItem in items)
      {
        string[] array = listItem.Value.Split('|');
        if (array.Length > 1)
        {
          listString.Add(array[1]);
        }
      }
      return listString.ToString();
    }

    /// <summary>
    ///   Sets the value.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <contract>
    ///   <requires name="text" condition="not null" />
    /// </contract>
    public void SetValue(string text)
    {
      Assert.ArgumentNotNull(text, "text");
      this.Value = text;
    }

    /// <summary>
    ///   Sets the modified.
    /// </summary>
    protected static void SetModified()
    {
      Sitecore.Context.ClientPage.Modified = true;
    }

    /// <summary>
    ///   Determines whether an item is based on a template from <paramref name="templateList" />.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="templateList">The template list - a set of comma-separated template names.</param>
    /// <returns>
    ///   <c>true</c> if item is based on a template, which name is mentioned in <paramref name="templateList" />; otherwise, <c>false</c>.
    /// </returns>
    /// <contract>
    ///   <requires name="item" condition="none" />
    ///   <requires name="templateList" condition="not null" />
    /// </contract>
    private static bool HasItemTemplate(Item item, string templateList)
    {
      Assert.ArgumentNotNull(templateList, "templateList");
      if (item == null)
      {
        return false;
      }
      if (templateList.Length == 0)
      {
        return false;
      }
      string[] array = templateList.Split(',');
      ArrayList arrayList = new ArrayList(array.Length);
      for (int i = 0; i < array.Length; i++)
      {
        arrayList.Add(array[i].Trim().ToLowerInvariant());
      }
      if (arrayList.Contains(item.TemplateName.Trim().ToLowerInvariant()))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    ///   Can be used after <c>OnLoad()</c> is called.
    ///   Fulfills parsing Of Value and restores <c>Listbox</c> state.
    /// </summary>
    /// <returns></returns>
    /// <contract>
    ///   <ensures condition="not null" />
    /// </contract>
    protected virtual string FormTemplateFilterForDisplay()
    {
      return this.FilterQueryBuilder.BuildFilterQuery(this);
    }

    /// <summary>
    ///   Determines whether an item is based on a template that is mentioned in <see cref="P:Sitecore.Shell.Applications.ContentEditor.TreeList.ExcludeTemplatesForSelection" />.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    ///   <c>true</c> if item is based on a template that is mentioned in <see cref="P:Sitecore.Shell.Applications.ContentEditor.TreeList.ExcludeTemplatesForSelection" />; otherwise, <c>false</c>.
    /// </returns>
    /// <contract>
    ///   <requires name="item" condition="none" />
    /// </contract>
    private bool HasExcludeTemplateForSelection(Item item)
    {
      if (item != null)
      {
        return TreeList.HasItemTemplate(item, this.ExcludeTemplatesForSelection);
      }
      return true;
    }

    /// <summary>
    ///   Determines whether [has include template for selection] [the specified item].
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    ///   <c>true</c> if [has include template for selection] [the specified item]; otherwise, <c>false</c>.
    /// </returns>
    /// <contract>
    ///   <requires name="item" condition="not null" />
    /// </contract>
    private bool HasIncludeTemplateForSelection(Item item)
    {
      Assert.ArgumentNotNull(item, "item");
      if (this.IncludeTemplatesForSelection.Length == 0)
      {
        return true;
      }
      return TreeList.HasItemTemplate(item, this.IncludeTemplatesForSelection);
    }

    /// <summary>
    ///   Determines whether this instance denies multiple selection.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="listbox">The <c>listbox</c>.</param>
    /// <returns>
    ///   <c>true</c> if this instance denies multiple selection; otherwise, <c>false</c>.
    /// </returns>
    private bool IsDeniedMultipleSelection(Item item, Listbox listbox)
    {
      Assert.ArgumentNotNull(listbox, "listbox");
      if (item == null)
      {
        return true;
      }
      if (this.AllowMultipleSelection)
      {
        return false;
      }
      foreach (ListItem control in listbox.Controls)
      {
        string[] array = control.Value.Split('|');
        if (array.Length >= 2 && array[1] == item.ID.ToString())
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    ///   Restores the state.
    /// </summary>
    private void RestoreState()
    {
      string[] array = this.Value.Split('|');
      if (array.Length != 0)
      {
        Database database = Sitecore.Context.ContentDatabase;
        if (!string.IsNullOrEmpty(this.DatabaseName))
        {
          database = Factory.GetDatabase(this.DatabaseName);
        }
        foreach (string text in array)
        {
          if (!string.IsNullOrEmpty(text))
          {
            ListItem listItem = new ListItem
            {
              ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("I")
            };
            this._listBox.Controls.Add(listItem);
            listItem.Value = listItem.ID + "|" + text;
            Item item = database.GetItem(text, Language.Parse(this.ItemLanguage));
            if (item != null)
            {
              listItem.Header = this.GetHeaderValue(item);
            }
            else
            {
              listItem.Header = text + " " + Translate.Text("[Item not found]");
            }
          }
        }
        SheerResponse.Refresh(this._listBox);
      }
    }

    /// <summary>
    ///   Sets the properties.
    /// </summary>
    private void SetProperties()
    {
      string @string = StringUtil.GetString(this.Source);
      if (@string.StartsWith("query:"))
      {
        if (Sitecore.Context.ContentDatabase != null && this.ItemID != null)
        {
          Item item = Sitecore.Context.ContentDatabase.GetItem(this.ItemID);
          if (item != null)
          {
            Item item2 = null;
            try
            {
              item2 = LookupSources.GetItems(item, @string).FirstOrDefault();
            }
            catch (Exception exception)
            {
              Log.Error("Treelist field failed to execute query.", exception, this);
            }
            if (item2 != null)
            {
              this.DataSource = item2.Paths.FullPath;
            }
          }
        }
      }
      else if (Sitecore.Data.ID.IsID(@string))
      {
        this.DataSource = this.Source;
      }
      else if (this.Source != null && !@string.Trim().StartsWith("/", StringComparison.OrdinalIgnoreCase))
      {
        this.ExcludeTemplatesForSelection = StringUtil.ExtractParameter("ExcludeTemplatesForSelection", this.Source).Trim();
        this.IncludeTemplatesForSelection = StringUtil.ExtractParameter("IncludeTemplatesForSelection", this.Source).Trim();
        this.IncludeTemplatesForDisplay = StringUtil.ExtractParameter("IncludeTemplatesForDisplay", this.Source).Trim();
        this.ExcludeTemplatesForDisplay = StringUtil.ExtractParameter("ExcludeTemplatesForDisplay", this.Source).Trim();
        this.ExcludeItemsForDisplay = StringUtil.ExtractParameter("ExcludeItemsForDisplay", this.Source).Trim();
        this.IncludeItemsForDisplay = StringUtil.ExtractParameter("IncludeItemsForDisplay", this.Source).Trim();
        string strA = StringUtil.ExtractParameter("AllowMultipleSelection", this.Source).Trim().ToLowerInvariant();
        this.AllowMultipleSelection = (string.Compare(strA, "yes", StringComparison.InvariantCultureIgnoreCase) == 0);
        this.DataSource = StringUtil.ExtractParameter("DataSource", this.Source).Trim().ToLowerInvariant();
        this.DatabaseName = StringUtil.ExtractParameter("databasename", this.Source).Trim().ToLowerInvariant();
      }
      else
      {
        this.DataSource = this.Source;
      }
    }
  }
}