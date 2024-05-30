import type { IRankingQuestionOption } from '@/entities/RankingQuestionOption';

export interface IRankingQuestion {
  identifier?: string;
  title?: string;
  maxSelectableItems?: number;
  status?: 'open' | 'closed';
  responseCount?: number;
  created?: string;
  options?: IRankingQuestionOption[];
}
