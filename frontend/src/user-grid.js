ej.base.registerLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXpecnRQR2hfU0RwXURWYE4=");

ej.grids.Grid.Inject(ej.grids.Page, ej.grids.Sort, ej.grids.Filter);

var grid = new ej.grids.Grid({
    columns: [
        { field: "Name", headerText: "Name", width: 120 },
        { field: "Department", headerText: "Department", width: 150 },
        { field: "Email", headerText: "Email", width: 200 },
    ],
    allowPaging: true,
    allowSorting: true,
    allowFiltering: true,
    filterSettings: { type: "Menu" }, // or "FilterBar", "Excel"
    pageSettings: { pageSize: 10 },
});
grid.appendTo("#user-grid");

globalThis.createPlainJsObject = () => {
    return {};
};

globalThis.addRecordToGrid = function (record) {
    grid.dataSource.push(record);
    grid.refresh();
};
