ej.base.registerLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXpecnRQR2hfU0RwXURWYE4=");

ej.grids.Grid.Inject(ej.grids.Page, ej.grids.Sort, ej.grids.Filter);

var grid = new ej.grids.Grid({
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

globalThis.createPlainJsObject = () => {
    return {};
};

globalThis.createPlainJsArray = () => {
    return [];
};

globalThis.handleUsersReady = () => {
    class CustomAdaptor extends ej.data.Adaptor {
        processQuery(_, query, __) {
            const skipObj = query.queries.find(q => q.fn === "onSkip");
            const takeObj = query.queries.find(q => q.fn === "onTake");

            console.log("skip/take", { skipObj: skipObj, takeObj: takeObj });

            const skip = skipObj ?? 0;
            const top = takeObj ?? 10;

            console.log("Querying data: skip=", skip, "top=", top);

            return new Promise((resolve, reject) => {
                try {
                    var result = $.FetchGridData(skip, top);

                    console.log("Query result:", result);

                    resolve({
                        result: result.data,
                        count: result.count,
                    });
                } catch (ex) {
                    console.error("Error fetching grid data:", ex);
                    reject(ex);
                }
            });
        }
    }

    const dataManager = new ej.data.DataManager({
        adaptor: new CustomAdaptor(),
    });

    grid.dataSource = dataManager;
    grid.refresh();
};
