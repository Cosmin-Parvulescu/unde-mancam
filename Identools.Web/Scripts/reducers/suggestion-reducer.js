var SuggestionsReducer = function (suggestions, action) {
  suggestions = typeof suggestions !== 'undefined' ? suggestions : [];

  switch (action.type) {
    case 'GOT_SUGGESTIONS':
      return action.suggestions.sort();

    case 'POSTED_SUGGESTION':
      return suggestions.concat(action.suggestion).sort();

    case 'UPDATE_SUGGESTION':
      var tempSuggestions = suggestions.filter(function (suggestion) { return suggestion.Id !== action.suggestion.Id; });
      tempSuggestions.push(action.suggestion);

      return tempSuggestions.sort();

    default:
      return suggestions;
  }
};