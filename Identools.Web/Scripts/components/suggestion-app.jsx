var sortDescriptor = function (a, b) {
    if (a.Location < b.Location)
        return -1;
    if (a.Location > b.Location)
        return 1;

    return 0;
}

var SuggestionApp = React.createClass({
    getInitialState: function () {
        return { suggestions: [] };
    },
    componentDidMount: function () {
        var suggestionHub = $.connection.suggestionHub;
        suggestionHub.client.addSuggestion = function (suggestion) {
            var newSuggestions = this.state.suggestions.slice();
            newSuggestions.push(suggestion);

            this.setState({
                suggestions: newSuggestions.sort(sortDescriptor)
            });
        }.bind(this);
        suggestionHub.client.updateSuggestion = function (updatedSuggestion) {
            var newSuggestions = this.state.suggestions.filter(function (suggestion) {
                return suggestion.Id !== updatedSuggestion.Id;
            });
            newSuggestions.push(updatedSuggestion);

            this.setState({
                suggestions: newSuggestions.sort(sortDescriptor)
            });
        }.bind(this);

        $.ajax({
            url: 'api/suggestions',
            type: 'GET',
            success: function (suggestions) {
                this.setState({
                    suggestions: suggestions.sort(sortDescriptor)
                });
            }.bind(this)
        });
    },
    render: function () {
        return (
            <div className="suggestion-app row extended">
                <SuggestionList suggestions={ this.state.suggestions } />
            </div>
        );
    }
});