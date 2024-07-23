export interface IQuestionOption {
  identifier?: string;
  title: string;
  description?: string;
  voters?: IQuestionOptionVoters[];
}

export interface IQuestionOptionVoters {
  identifier: string;
  username: string;
}
