var SuggestionsReducer = function (suggestions, action) {
  suggestions = typeof suggestions !== 'undefined' ? suggestions : [];

  switch (action.type) {
    case 'GOT_SUGGESTIONS':
      return action.suggestions.sort();

    case 'POSTED_SUGGESTION':
      return suggestions.concat(action.suggestion).sort();

    default:
      return suggestions;
  }
};