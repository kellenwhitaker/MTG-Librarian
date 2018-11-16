// Credit: Marc Gravell
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Reflection;

public class ObjectShredder<T>
{
    private System.Reflection.FieldInfo[] _fi;
    private System.Reflection.PropertyInfo[] _pi;
    private System.Collections.Generic.Dictionary<string, int> _ordinalMap;
    private System.Type _type;

    // ObjectShredder constructor.
    public ObjectShredder()
    {
        _type = typeof(T);
        _fi = _type.GetFields();
        _pi = _type.GetProperties();
        _ordinalMap = new Dictionary<string, int>();
    }

    /// <summary>
    /// Loads a DataTable from a sequence of objects.
    /// </summary>
    /// <param name="source">The sequence of objects to load into the DataTable.</param>
    /// <param name="table">The input table. The schema of the table must match that 
    /// the type T.  If the table is null, a new table is created with a schema 
    /// created from the public properties and fields of the type T.</param>
    /// <param name="options">Specifies how values from the source sequence will be applied to 
    /// existing rows in the table.</param>
    /// <returns>A DataTable created from the source sequence.</returns>
    public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
    {
        // Load the table from the scalar sequence if T is a primitive type.
        if (typeof(T).IsPrimitive)
        {
            return ShredPrimitive(source, table, options);
        }

        // Create a new table if the input table is null.
        if (table == null)
        {
            table = new DataTable(typeof(T).Name);
        }

        // Initialize the ordinal map and extend the table schema based on type T.
        table = ExtendTable(table, typeof(T));

        // Enumerate the source sequence and load the object values into rows.
        table.BeginLoadData();
        using (IEnumerator<T> e = source.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (options != null)
                {
                    table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
                }
                else
                {
                    table.LoadDataRow(ShredObject(table, e.Current), true);
                }
            }
        }
        table.EndLoadData();

        // Return the table.
        return table;
    }

    public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
    {
        // Create a new table if the input table is null.
        if (table == null)
        {
            table = new DataTable(typeof(T).Name);
        }

        if (!table.Columns.Contains("Value"))
        {
            table.Columns.Add("Value", typeof(T));
        }

        // Enumerate the source sequence and load the scalar values into rows.
        table.BeginLoadData();
        using (IEnumerator<T> e = source.GetEnumerator())
        {
            Object[] values = new object[table.Columns.Count];
            while (e.MoveNext())
            {
                values[table.Columns["Value"].Ordinal] = e.Current;

                if (options != null)
                {
                    table.LoadDataRow(values, (LoadOption)options);
                }
                else
                {
                    table.LoadDataRow(values, true);
                }
            }
        }
        table.EndLoadData();

        // Return the table.
        return table;
    }

    public object[] ShredObject(DataTable table, T instance)
    {

        FieldInfo[] fi = _fi;
        PropertyInfo[] pi = _pi;

        if (instance.GetType() != typeof(T))
        {
            // If the instance is derived from T, extend the table schema
            // and get the properties and fields.
            ExtendTable(table, instance.GetType());
            fi = instance.GetType().GetFields();
            pi = instance.GetType().GetProperties();
        }

        // Add the property and field values of the instance to an array.
        Object[] values = new object[table.Columns.Count];
        foreach (FieldInfo f in fi)
        {
            values[_ordinalMap[f.Name]] = f.GetValue(instance);
        }

        foreach (PropertyInfo p in pi)
        {
            values[_ordinalMap[p.Name]] = p.GetValue(instance, null);
        }

        // Return the property and field values of the instance.
        return values;
    }

    public DataTable ExtendTable(DataTable table, Type type)
    {
        // Extend the table schema if the input table was null or if the value 
        // in the sequence is derived from type T.            
        foreach (FieldInfo f in type.GetFields())
        {
            if (!_ordinalMap.ContainsKey(f.Name))
            {
                // Add the field as a column in the table if it doesn't exist
                // already.
                DataColumn dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name]
                    : table.Columns.Add(f.Name, f.FieldType);

                // Add the field to the ordinal map.
                _ordinalMap.Add(f.Name, dc.Ordinal);
            }
        }
        foreach (PropertyInfo p in type.GetProperties())
        {
            if (!_ordinalMap.ContainsKey(p.Name))
            {
                // Add the property as a column in the table if it doesn't exist
                // already.
                DataColumn dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name]
                    : table.Columns.Add(p.Name, p.PropertyType);

                // Add the property to the ordinal map.
                _ordinalMap.Add(p.Name, dc.Ordinal);
            }
        }

        // Return the table.
        return table;
    }
}

public static class IEnumerableExtensions
{
    public static AdvancedList<T> ToAdvancedList<T>(this IEnumerable<T> source)
    {
        return new AdvancedList<T>(source);
    }

    public static BindingList<T> ToBindingList<T>(this IList<T> source)
    {
        return new BindingList<T>(source);
    }
}

public class AdvancedList<T> : BindingList<T>, IBindingListView
{
    public AdvancedList(IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            Add(item);
        }
    }

    protected override bool IsSortedCore
    {
        get { return sorts != null; }
    }
    protected override void RemoveSortCore()
    {
        sorts = null;
    }
    protected override bool SupportsSortingCore
    {
        get { return true; }
    }
    protected override ListSortDirection SortDirectionCore
    {
        get
        {
            return sorts == null ? ListSortDirection.Ascending :
      sorts.PrimaryDirection;
        }
    }
    protected override PropertyDescriptor SortPropertyCore
    {
        get { return sorts == null ? null : sorts.PrimaryProperty; }
    }
    protected override void ApplySortCore(PropertyDescriptor prop,
    ListSortDirection direction)
    {
        ListSortDescription[] arr = {
new ListSortDescription(prop, direction)
};
        ApplySort(new ListSortDescriptionCollection(arr));
    }
    PropertyComparerCollection<T> sorts;
    public void ApplySort(ListSortDescriptionCollection
    sortCollection)
    {
        bool oldRaise = RaiseListChangedEvents;
        RaiseListChangedEvents = false;
        try
        {
            PropertyComparerCollection<T> tmp
            = new PropertyComparerCollection<T>(sortCollection);
            List<T> items = new List<T>(this);
            items.Sort(tmp);
            int index = 0;
            foreach (T item in items)
            {
                SetItem(index++, item);
            }
            sorts = tmp;
        }
        finally
        {
            RaiseListChangedEvents = oldRaise;
            ResetBindings();
        }
    }
    string IBindingListView.Filter
    {
        get { throw new NotImplementedException(); }
        set { throw new NotImplementedException(); }
    }
    void IBindingListView.RemoveFilter()
    {
        throw new NotImplementedException();
    }
    ListSortDescriptionCollection IBindingListView.SortDescriptions
    {
        get { return sorts.Sorts; }
    }
    bool IBindingListView.SupportsAdvancedSorting
    {
        get { return true; }
    }
    bool IBindingListView.SupportsFiltering
    {
        get { return false; }
    }
}
public class PropertyComparerCollection<T> : IComparer<T>
{
    private readonly ListSortDescriptionCollection sorts;
    private readonly PropertyComparer<T>[] comparers;
    public ListSortDescriptionCollection Sorts
    {
        get { return sorts; }
    }
    public PropertyComparerCollection(ListSortDescriptionCollection
    sorts)
    {
        if (sorts == null) throw new ArgumentNullException("sorts");
        this.sorts = sorts;
        List<PropertyComparer<T>> list = new
        List<PropertyComparer<T>>();
        foreach (ListSortDescription item in sorts)
        {
            list.Add(new PropertyComparer<T>(item.PropertyDescriptor,
            item.SortDirection == ListSortDirection.Descending));
        }
        comparers = list.ToArray();
    }
    public PropertyDescriptor PrimaryProperty
    {
        get
        {
            return comparers.Length == 0 ? null :
            comparers[0].Property;
        }
    }
    public ListSortDirection PrimaryDirection
    {
        get
        {
            return comparers.Length == 0 ? ListSortDirection.Ascending
            : comparers[0].Descending ?
            ListSortDirection.Descending
            : ListSortDirection.Ascending;
        }
    }

    int IComparer<T>.Compare(T x, T y)
    {
        int result = 0;
        for (int i = 0; i < comparers.Length; i++)
        {
            result = comparers[i].Compare(x, y);
            if (result != 0) break;
        }
        return result;
    }
}

public class PropertyComparer<T> : IComparer<T>
{
    private readonly bool descending;
    public bool Descending { get { return descending; } }
    private readonly PropertyDescriptor property;
    public PropertyDescriptor Property { get { return property; } }
    public PropertyComparer(PropertyDescriptor property, bool
    descending)
    {
        if (property == null) throw new
        ArgumentNullException("property");
        this.descending = descending;
        this.property = property;
    }
    public int Compare(T x, T y)
    {
        // todo; some null cases
        int value = Comparer.Default.Compare(property.GetValue(x),
        property.GetValue(y));
        return descending ? -value : value;
    }
}
