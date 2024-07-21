import type { IQuestionOption } from '@/entities/QuestionOption';

export interface IQuestion {
  identifier?: string;
  title?: string;
  type?: string;
  maxSelectableItems?: number;
  status?: 'open' | 'closed';
  responseCount?: number;
  created?: string;
  options?: IQuestionOption[];
  responses?: IQuestionResponse[];
}

export interface IQuestionResponse {
  identifier: string;
  username: string;
  choice: string[];
}
