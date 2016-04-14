var SuggestionsReducer = function (suggestions, action) {
  suggestions = typeof suggestions !== 'undefined' ? suggestions : [];

  switch (action.type) {
    case 'GOT_SUGGESTIONS':
      return action.suggestions;

    default:
      return suggestions;
  }
};