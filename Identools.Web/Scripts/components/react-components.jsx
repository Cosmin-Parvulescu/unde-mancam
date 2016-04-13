var sortDescriptor = function(a, b) {
    if (a.AttendeeCount > b.AttendeeCount)
        return -1;
    if (a.AttendeeCount < b.AttendeeCount)
        return 1;

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

var Suggestion = React.createClass({
    handleAttend: function () {
        $.ajax({
            url: 'api/suggestions/attend/' + this.props.suggestion.Id,
            type: 'GET'
        });
    },
    handleVote: function () {
        $.ajax({
            url: 'api/suggestions/vote/' + this.props.suggestion.Id,
            type: 'GET'
        });
    },
    render: function () {
        return (
            <div className="column small-4">
                <div className="suggestion">
                    <h2 className="suggestion-header">{ this.props.suggestion.Location }</h2>
                    <p className="suggestion-time">{ new Date(this.props.suggestion.StartTime).toLocaleTimeString() }</p>

                    <div className="row">
                        <div className="column small-4">
                            <span data-tooltip aria-haspopup="true" className="has-tip" title={ this.props.suggestion.Attendees }>{ this.props.suggestion.AttendeeCount } going!</span>
                        </div>

                        <div className="column small-2">
                            <button onClick={ this.handleAttend }>
                                <i className={ "fa fa-user-plus " + (this.props.suggestion.Attending ? 'green' : '') } aria-hidden="true"></i>
                            </button>
                        </div>

                        <div className="column small-2">
                            &nbsp;
                        </div>

                        <div className="column small-4">
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        );
    }
});

var SuggestionForm = React.createClass({
    getInitialState: function () {
        return {
            Location: undefined,
            StartTime: undefined
        }
    },
    componentDidMount: function () {
        var self = this;

        var startTimeEl = $('.suggestion-form input[name="StartTime"]');

        startTimeEl.timepicker({
            minTime: '11:00AM',
            maxTime: '02:00PM',
            disableTextInput: true
        });
        startTimeEl.on('changeTime', function () {
            self.setState({
                StartTime: $(this).val()
            });
        });
    },
    handleLocationChange: function (event) {
        this.setState({ Location: event.target.value });
    },
    handleStartTimeChange: function (event) {
        this.setState({ StartTime: event.target.value });
    },
    handleSubmit: function () {
        $.ajax({
            url: 'api/suggestions',
            type: 'POST',
            data: {
                Location: this.state.Location,
                StartTime: this.state.StartTime
            },
            contentType: 'application/x-www-form-urlencoded'
        }).success(function () {
            var initialState = this.getInitialState();
            this.setState(initialState);
        }.bind(this));
    },
    render: function () {
        return (
            <div className="suggestion-form column small-4 small-centered">
                <div className="input-group">
                    <input className="input-group-field" type="text" name="Location" placeholder="Unde?" value={ this.state.Location } onChange={ this.handleLocationChange } />

                    <span className="input-group-label">
                        <i className="fa fa-globe" aria-hidden="true"></i>
                    </span>
                </div>

                <div className="input-group">
                    <input className="input-group-field datetime-local" type="text" name="StartTime" placeholder="Când?" value={ this.state.StartTime } onChange={ this.handleStartTimeChange } />

                    <span className="input-group-label">
                        <i className="fa fa-clock-o" aria-hidden="true"></i>
                    </span>
                </div>


                <button className="button expanded" onClick={ this.handleSubmit }>Adaugă</button>
            </div>
        );
    }
});