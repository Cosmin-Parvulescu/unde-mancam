var SuggestionApp = React.createClass({
    render: function () {
        return (
            <div className="suggestion-app">
                <SuggestionForm />
                <SuggestionList />
            </div>
        );
    }
});

SuggestionApp = ReactRedux.connect()(SuggestionApp);