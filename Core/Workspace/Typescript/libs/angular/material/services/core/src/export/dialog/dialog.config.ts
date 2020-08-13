export type PromptType =  'string' | 'date';

export interface DialogConfig {
  title?: string;
  message?: string;
  label?: string;
  placeholder?: string;
  promptType?: PromptType;
}
