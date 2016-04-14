var SuggestionList = React.createClass({
    render: function () {
        var suggestions = this.props.suggestions.map(function (suggestion) {
            return (<Suggestion suggestion={ suggestion } />);
        });

        return (
            <div className="suggestion-list row">
                <div className="row">
                    <SuggestionForm />
                </div>

                { suggestions }
            </div>
        );
    }
});