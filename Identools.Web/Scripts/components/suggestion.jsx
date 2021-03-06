﻿var Suggestion = React.createClass({
    handleAttend: function() {
        this.props.handleAttend(this.props.suggestion.Id);
    },
    getLocalDate: function(utcDateString) {
        var dateToConvert = new Date(utcDateString + 'Z');

        dateToConvert.setFullYear(dateToConvert.getUTCFullYear(), dateToConvert.getUTCMonth(), dateToConvert.getUTCDate());
        dateToConvert.setHours(dateToConvert.getUTCHours(), dateToConvert.getUTCMinutes(), dateToConvert.getUTCSeconds(), 0);
        return dateToConvert;
    },
    render: function() {
        return (
            <div className="column small-12 medium-6 large-4">
                <div className="suggestion">
                    <h2 className="suggestion-header">{ this.props.suggestion.Location }</h2>
                    <p className="suggestion-time">{ this.getLocalDate(this.props.suggestion.StartTime).toLocaleTimeString() }</p>

                    <div className="row">
                        <div className="column">
                            <span data-tooltip aria-haspopup="true" className="has-tip" title={ this.props.suggestion.Attendees }>{ this.props.suggestion.AttendeeCount } going!</span>
                        </div>

                        <div className="column">
                            <button onClick={ this.handleAttend }>
                                <i className={ "fa fa-user-plus " + (this.props.suggestion.Attending ? 'green' : '') } aria-hidden="true"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
});

var SuggestionMapDispatchToProps = function () {
    return {
        handleAttend: function (suggestionId) {
            $.ajax({
                url: 'api/suggestions/attend/' + suggestionId,
                type: 'GET'
            });
        }
    };
};

Suggestion = ReactRedux.connect(null, SuggestionMapDispatchToProps)(Suggestion);