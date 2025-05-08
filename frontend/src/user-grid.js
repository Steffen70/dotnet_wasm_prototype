ej.base.registerLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXpecnRQR2hfU0RwXURWYE4=");

ej.grids.Grid.Inject(ej.grids.Page, ej.grids.Sort, ej.grids.Filter);

class WasmDataAdaptor extends ej.data.Adaptor {
    constructor() {
        super();
        this.ready = false;
    }

    selectFetchFunction(where, sort) {
        if (where && sort) return (where, select, skip, take) => $.FetchFilteredAndSortedData(where, select, skip, take);
        if (where) return (where, _, skip, take) => $.FetchFilteredData(where, skip, take);
        if (sort) return (_, select, skip, take) => $.FetchSortedData(select, skip, take);
        return (_, __, skip, take) => $.FetchPaginatedData(skip, take);
    }

    processQuery(_, query) {
        if (!this.ready) return { result: [], count: 0 };

        // console.log("Processing Query:", query.queries);

        const page = query.queries.find(q => q.fn === "onPage")?.e;
        const where = query.queries.find(q => q.fn === "onWhere")?.e?.predicates;
        const sort = query.queries.find(q => q.fn === "onSortBy")?.e;

        // console.log("Page Query:", page);
        // console.log("Where Query:", where);
        // console.log("Sort Query:", sort);

        if (!page && !where && !sort) return { result: [], count: 0 };

        let skip = 0;
        let take = 5;

        if (page) {
            skip = (page.pageIndex - 1) * page.pageSize;
            take = page.pageSize;
        }

        const fetchFn = this.selectFetchFunction(where, sort);
        const result = fetchFn(where, sort, skip, take);

        return {
            result: Array.from(result.Data),
            count: result.Count,
        };
    }
}

let grid = new ej.grids.Grid({
    dataSource: new ej.data.DataManager({
        adaptor: new WasmDataAdaptor(),
    }),
    columns: [
        { field: "Name", headerText: "Name", width: 120, isPrimaryKey: true },
        { field: "Department", headerText: "Department", width: 150 },
        { field: "Email", headerText: "Email", width: 200 },
    ],
    allowPaging: true,
    allowSorting: true,
    allowFiltering: true,
    filterSettings: { type: "Menu" },
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
