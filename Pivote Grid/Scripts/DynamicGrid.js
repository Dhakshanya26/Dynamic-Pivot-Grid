/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/knockout/knockout.d.ts" />
/// <reference path="typings/knockout.validation/knockout.validation.d.ts" />
//Dynamic Grid Container
var PivoteModel = (function () {
    function PivoteModel(gridData) {
        var _this = this;
        this.searchByButtonClick = function () {
            _this.showLoadingIcon(true);
            window.location.href = window.location.pathname + '?searchTerm=' + _this.searchByValue();
        };
        this.loadGridData = function (gridData) {
            _this.gridRows = ko.observable(gridData.RowModels);
            _this.gridColumns = ko.observable(gridData.ColumnModel);
            var self = _this;
            debugger;
            var div = Math.floor(_this.totalCounts() / _this.pageSize());
            div += _this.totalCounts() % _this.pageSize() > 0 ? 1 : 0;
            console.log(div);
            self.maxPages(div);
        };
        this.loadFirstPage = function () {
            _this.showLoadingIcon(true);
            $.ajax({
                url: _this.pagingUrl,
                type: "POST",
                data: {
                    pageNumber: 1,
                    pageSize: _this.pageSize(),
                    sortByColumn: _this.sortByColumn(),
                    sortByRow: _this.sortByRow(),
                    searchTerm: _this.searchByValue()
                },
                dataType: "json"
            })
                .done(function (result) {
                _this.pageNumber(1);
                _this.reloadData(result);
                _this.showLoadingIcon(false);
            });
        };
        this.reloadData = function (gridData) {
            _this.gridRows(gridData.RowModels);
            _this.gridColumns(gridData.ColumnModel);
        };
        this.loadLastPage = function () {
            _this.showLoadingIcon(true);
            $.ajax({
                url: _this.pagingUrl,
                type: "POST",
                data: {
                    pageNumber: _this.maxPages(), pageSize: _this.pageSize(),
                    sortByColumn: _this.sortByColumn(),
                    sortByRow: _this.sortByRow(),
                    searchTerm: _this.searchByValue()
                },
                dataType: "json"
            })
                .done(function (result) {
                _this.pageNumber(_this.maxPages());
                _this.reloadData(result);
                _this.showLoadingIcon(false);
            });
        };
        this.loadNextPage = function () {
            _this.showLoadingIcon(true);
            $.ajax({
                url: _this.pagingUrl,
                type: "POST",
                data: {
                    pageNumber: _this.pageNumber() + 1, pageSize: _this.pageSize(),
                    sortByColumn: _this.sortByColumn(),
                    sortByRow: _this.sortByRow(),
                    searchTerm: _this.searchByValue()
                },
                dataType: "json"
            })
                .done(function (result) {
                _this.showLoadingIcon(false);
                _this.pageNumber(_this.pageNumber() + 1);
                _this.reloadData(result);
            });
        };
        this.dataClickEvent = function (rowValue, data, event) {
            debugger;
            alert('I am ' + rowValue + ' Id: ' + data.Id + ' Value : ' + data.Name);
        };
        this.loadPreviousPage = function () {
            _this.showLoadingIcon(true);
            $.ajax({
                url: _this.pagingUrl,
                type: "POST",
                data: {
                    pageNumber: _this.pageNumber() - 1, pageSize: _this.pageSize(),
                    sortByColumn: _this.sortByColumn(),
                    sortByRow: _this.sortByRow(),
                    searchTerm: _this.searchByValue()
                },
                dataType: "json"
            })
                .done(function (result) {
                _this.pageNumber(_this.pageNumber() - 1);
                _this.showLoadingIcon(false);
                _this.reloadData(result);
            });
        };
        this.sortDataGrid = function () {
            _this.showLoadingIcon(true);
            $.ajax({
                url: _this.pagingUrl,
                type: "POST",
                data: {
                    pageNumber: _this.pageNumber(), pageSize: _this.pageSize(), sortByColumn: _this.sortByColumn(),
                    sortByRow: _this.sortByRow(), searchTerm: _this.searchByValue()
                },
                dataType: "json"
            })
                .done(function (result) {
                _this.showLoadingIcon(false);
                _this.reloadData(result);
            });
            return true;
        };
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
    return PivoteModel;
}());
//# sourceMappingURL=DynamicGrid.js.map