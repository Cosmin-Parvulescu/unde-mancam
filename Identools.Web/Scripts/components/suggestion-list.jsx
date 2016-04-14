var SuggestionList = React.createClass({
    componentDidMount: function () {
        this.props.onComponentDidMount();
    },
    render: function () {
        var suggestions = this.props.suggestions.map(function (suggestion) { return <p key={ suggestion.Id }>{ suggestion.Location}</p> });
        return <div>{ suggestions }</div>;
    }
});

var SuggestionListMapStateToProps = function (suggestions) {
    return {
        suggestions: suggestions
    };
};

var SuggestionListMapDispatchToProps = function (dispatch) {
    return {
        onComponentDidMount: function () {
            dispatch(function (dispatch) {
                $.ajax({
                    url: 'api/suggestions',
                    type: 'GET'
                }).success(function (suggestions) {
                    return dispatch({
                        type: 'GOT_SUGGESTIONS',
                        suggestions: suggestions
                    });
                });
            });
        }
    };
};

SuggestionList = ReactRedux.connect(SuggestionListMapStateToProps, SuggestionListMapDispatchToProps)(SuggestionList);