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
                suggestions: newSuggestions
            });
        }.bind(this);

        $.ajax({
            url: 'api/suggestions',
            type: 'GET',
            success: function (suggestions) {
                this.setState({
                    suggestions: suggestions
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
                { suggestions }

                <SuggestionForm />
            </div>
        );
    }
});

var Suggestion = React.createClass({
    render: function () {
        return (
            <div className="column small-4">
                <div className="suggestion">
                    <h2 className="suggestion-header">{ this.props.suggestion.Location }</h2>
                    <p className="suggestion-time">{ new Date(this.props.suggestion.StartTime).toLocaleTimeString() }</p>

                    <div className="row">
                        <div className="column small-1">

                        </div>

                        <div className="column small-1">

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
            this.setState(this.getInitialState());
        });
    },
    render: function () {
        return (
            <div className="suggestion-form column small-4">
                <h3>Adaugă o sugestie</h3>
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


                <button className="button full-width" onClick={ this.handleSubmit }>Adaugă</button>
            </div>
        );
    }
});