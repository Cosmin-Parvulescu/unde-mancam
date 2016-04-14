var SuggestionApp = React.createClass({
    render: function () {
        return <SuggestionList />;
    }
});

SuggestionApp = ReactRedux.connect()(SuggestionApp);