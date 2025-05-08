ej.base.registerLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXpecnRQR2hfU0RwXURWYE4=");

ej.grids.Grid.Inject(ej.grids.Page, ej.grids.Sort, ej.grids.Filter);

class WasmDataAdaptor extends ej.data.Adaptor {
    constructor(fetchData) {
        super();
        this.ready = false;
        this.fetchData = fetchData;
    }

    processQuery(_, query) {
        if (!this.ready) return { result: [], count: 0 };

        const pageQuery = query.queries.find(q => q.fn === "onPage").e;
        let result = this.fetchData((pageQuery.pageIndex - 1) * pageQuery.pageSize, pageQuery.pageSize);

        return {
            result: Array.from(result.Data),
            count: result.Count,
        };
    }
}

let grid = new ej.grids.Grid({
    dataSource: new ej.data.DataManager({
        adaptor: new WasmDataAdaptor((skip, take) => $.FetchGridData(skip, take)),
    }),
    columns: [
        { field: "Name", headerText: "Name", width: 120, isPrimaryKey: true },
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

globalThis.handleFirstPageReady = _ => {
    grid.dataSource.adaptor.ready = true;
    grid.refresh();
};

globalThis.handleStreamEnd = (_, totalCount) => {
    grid.pagerModule.pageSettings.totalRecordsCount = totalCount;
};
