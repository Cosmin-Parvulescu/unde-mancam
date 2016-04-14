var Suggestion = React.createClass({
    render: function () {
        return (
            <div className="column small-12 medium-6 large-4">
                <div className="suggestion">
                    <h2 className="suggestion-header">{ this.props.suggestion.Location }</h2>
                    <p className="suggestion-time">{ new Date(this.props.suggestion.StartTime).toLocaleTimeString() }</p>

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