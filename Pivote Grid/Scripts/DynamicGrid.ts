/// <reference path="typings/jquery/jquery.d.ts" />

/// <reference path="typings/knockout/knockout.d.ts" />

/// <reference path="typings/knockout.validation/knockout.validation.d.ts" />

//Dynamic Grid Container

 
class PivoteModel {
    public gridColumns: KnockoutObservable<any>;
    public gridRows: KnockoutObservable<any>;
    pageNumber: KnockoutObservable<number>;
    pageSize: KnockoutObservable<number>;
    maxPages: KnockoutObservable<number>;
    totalCounts: KnockoutObservable<number>;
    pagingUrl: string;
    showNoRecordsFound: KnockoutObservable<boolean>;
    showLoadingIcon: KnockoutObservable<Boolean>;
    searchByValue: KnockoutObservable<string>;
    sortByRow: KnockoutObservable<boolean>;
    sortByColumn: KnockoutObservable<boolean>;
    columnHeading: string;
    rowHeading: string;
    showSearchByBox: KnockoutObservable<boolean>;
    constructor(gridData) {
        this.maxPages = ko.observable(0);
        this.sortByRow = ko.observable(false);
        this.sortByColumn = ko.observable(false);
        this.columnHeading = gridData.ColumnHeading;
        this.rowHeading = gridData.RowHeading;

        this.pageNumber = ko.observable(gridData.PagingRequest.PageNumber);
        this.pageSize = ko.observable(gridData.PagingRequest.PageSize);
        this.totalCounts = ko.observable(gridData.PagingRequest.TotalRecord);
        this.loadGridData(gridData);
        this.searchByValue = ko.observable(gridData.PagingRequest.SearchTerm == null ? "" : gridData.PagingRequest.SearchTerm);
        this.showNoRecordsFound = ko.observable(gridData.RowModels != null && gridData.RowModels.length <= 0);
        this.pagingUrl = gridData.PagingRequest.PagingUrl;
        this.showLoadingIcon = ko.observable(false);
        this.showSearchByBox = ko.observable(true);
        this.searchByValue =
            ko.observable(gridData.PagingRequest.SearchTerm == null ? "" : gridData.PagingRequest.SearchTerm);

    }
    searchByButtonClick = () => {
        this.showLoadingIcon(true);
        window.location.href = window.location.pathname + '?searchTerm=' + this.searchByValue();
    }
    loadGridData = (gridData) => {
      
        this.gridRows = ko.observable(gridData.RowModels);
        this.gridColumns = ko.observable(gridData.ColumnModel);
        var self = this;
        debugger;
        var div = Math.floor(this.totalCounts() / this.pageSize());
        div += this.totalCounts() % this.pageSize() > 0 ? 1 : 0;
        
        console.log(div);
        self.maxPages(div);
    }

    loadFirstPage = () => {
        this.showLoadingIcon(true);
        $.ajax({
            url: this.pagingUrl,
            type: "POST",
            data: {
                pageNumber: 1,
                pageSize: this.pageSize(),
                sortByColumn: this.sortByColumn(),
                sortByRow: this.sortByRow(),
                searchTerm: this.searchByValue()
            },
            dataType: "json"
        })
            .done(result => {
                this.pageNumber(1);
                this.reloadData(result);
                this.showLoadingIcon(false);
            });

    }

    reloadData = (gridData) => {
        this.gridRows(gridData.RowModels);
        this.gridColumns(gridData.ColumnModel);
    };

    loadLastPage = () => {
        this.showLoadingIcon(true);
        $.ajax({
            url: this.pagingUrl,
            type: "POST",
            data: {
                pageNumber: this.maxPages(), pageSize: this.pageSize(),
                sortByColumn: this.sortByColumn(),
                sortByRow: this.sortByRow(),
                searchTerm: this.searchByValue()
            },
            dataType: "json"
        })
            .done(result => {
                this.pageNumber(this.maxPages());
                this.reloadData(result);
                this.showLoadingIcon(false);
            });

    }
    loadNextPage = () => {
        this.showLoadingIcon(true);
        $.ajax({
            url: this.pagingUrl,
            type: "POST",
            data: {
                pageNumber: this.pageNumber() + 1, pageSize: this.pageSize(),
                sortByColumn: this.sortByColumn(),
                sortByRow: this.sortByRow(),
                searchTerm: this.searchByValue()
            },
            dataType: "json"
        })
            .done(result => {

                this.showLoadingIcon(false);
                this.pageNumber(this.pageNumber() + 1);
                this.reloadData(result);
            });

    }
    dataClickEvent = (rowValue,data, event) => {
        debugger;
        alert('I am ' + rowValue  + ' Id: ' + data.Id + ' Value : ' + data.Name );
    }

    loadPreviousPage = () => {
        this.showLoadingIcon(true);
        $.ajax({
            url: this.pagingUrl,
            type: "POST",
            data: {
                pageNumber: this.pageNumber() - 1, pageSize: this.pageSize(),
                sortByColumn: this.sortByColumn(),
                sortByRow: this.sortByRow(),
                searchTerm: this.searchByValue()
            },
            dataType: "json"
        })
            .done(result => {
                this.pageNumber(this.pageNumber() - 1);
                this.showLoadingIcon(false);
                this.reloadData(result);
            });

    }
    sortDataGrid = () => {
        this.showLoadingIcon(true);
        $.ajax({
                url: this.pagingUrl,
                type: "POST",
                data: {
                    pageNumber: this.pageNumber(), pageSize: this.pageSize(), sortByColumn: this.sortByColumn(),
                    sortByRow: this.sortByRow(), searchTerm: this.searchByValue() },
                dataType: "json"
            })
            .done(result => {
                this.showLoadingIcon(false);
                this.reloadData(result);
            });
        return true;
    };

}
