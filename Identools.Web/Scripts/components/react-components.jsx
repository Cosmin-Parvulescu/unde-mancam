var SuggestionApp = React.createClass({
    getInitialState: function () {
        return { suggestions: [] };
    },
    componentDidMount: function () {
        $.ajax({
            url: 'api/suggestions',
            type: 'GET',
            success: function (suggestions) {
                this.setState({ suggestions: suggestions });

                var suggestionHub = $.connection.suggestionHub;
                suggestionHub.client.updateSuggestions = function () {
                    $.ajax({
                        url: 'api/suggestions',
                        type: 'GET',
                        success: function (suggestions) {
                            this.setState({ suggestions: suggestions });
                        }.bind(this)
                    });
                }.bind(this);
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

var SuggestionList = React.createClass({
    render: function () {
        var suggestions = this.props.suggestions.map(function (suggestion) {
            return (<Suggestion suggestion={ suggestion } />);
        });

        return (
            <div className="suggestion-list row">
                { suggestions }
            </div>
        );
    }
});

var Suggestion = React.createClass({
    render: function () {
        return (
            <div className="suggestion column small-4">
                <h2>{ this.props.suggestion.Location }</h2>
                <p> { this.props.suggestion.UserName }</p>
                <p> { new Date(this.props.suggestion.StartTime).toLocaleTimeString() }</p>
            </div>
        );
    }
});