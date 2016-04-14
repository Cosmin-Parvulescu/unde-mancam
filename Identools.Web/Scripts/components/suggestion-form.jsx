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
            <div className="suggestion-form column small-12 medium-6 large-4 small-centered">
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