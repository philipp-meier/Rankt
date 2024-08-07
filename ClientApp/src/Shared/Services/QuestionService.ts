import { API_ENDPOINTS } from '@/ApiEndpoints';

export class QuestionService {
  static hasVoted(questionId: string, username: string): boolean {
    const state = this.getState();
    if (!state || !state[questionId]) return false;

    return state[questionId].find((x: any) => x.username === username)?.voted == true;
  }

  static setVoted(questionId: string, username: string): void {
    let state = this.getState();

    if (!state) state = {};

    if (!(questionId in state)) state[questionId] = [];

    const questionState = state[questionId].find((x: any) => x.username === username);
    if (questionState) questionState.voted = true;
    else state[questionId].push({ username: username, voted: true });

    this.setState(state);
  }

  private static getState(): UserState {
    const stateObj = localStorage.getItem('userState');
    return stateObj ? JSON.parse(stateObj) : null;
  }

  private static setState(state: UserState) {
    localStorage.setItem('userState', JSON.stringify(state));
  }

  public static async getResult(questionIdentifier: string): Promise<any | null> {
    const questionAvailableResponse = await fetch(
      `${API_ENDPOINTS.Questions}/${questionIdentifier!}/result`,
      {
        method: 'HEAD'
      }
    );

    if (!questionAvailableResponse.ok) return null;

    const resp = await fetch(`${API_ENDPOINTS.Questions}/${questionIdentifier!}/result`, {
      method: 'GET'
    });
    return await resp.json();
  }
}

type UserState = Record<string, QuestionState[]>;

type QuestionState = {
  username: string;
  voted: boolean;
};
