import { Injectable } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { format, setDay, setMonth, getDaysInMonth, parse, toDate, addYears, addMonths, addDays } from 'date-fns';

import { DateService } from '@allors/angular/services/core';

const ISO_8601_REGEX = /^\d{4}-\d{2}-\d{2}(?:T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|(?:(?:\+|-)\d{2}:\d{2}))?)?$/;

@Injectable()
export class AllorsDateAdapter extends DateAdapter<string> {
  constructor(private dateService: DateService) {
    super();
  }

  getYear(date: string): number {
    return new Date(date).getUTCFullYear();
  }

  getMonth(date: string): number {
    return new Date(date).getUTCMonth();
  }

  getDate(date: string): number {
    return new Date(date).getUTCDate();
  }

  getDayOfWeek(date: string): number {
    return new Date(date).getDay();
  }

  getMonthNames(style: 'long' | 'short' | 'narrow'): string[] {
    const map = {
      long: 'LLLL',
      short: 'LLL',
      narrow: 'LLLLL',
    };

    const formatStr = map[style];
    const date = new Date();

    return [...Array(12).keys()].map((month) =>
      format(setMonth(date, month), formatStr, {
        locale: this.dateService.locale,
      }),
    );
  }

  getDateNames(): string[] {
    return Array.from(Array(31), (_, i) => i + 1).map((day) => String(day));
  }

  getDayOfWeekNames(style: 'long' | 'short' | 'narrow'): string[] {
    const map = {
      long: 'EEEE',
      short: 'EEE',
      narrow: 'EEEEE',
    };

    const formatStr = map[style];
    const date = new Date();

    return [...Array(6).keys()].map((month) =>
      format(setDay(date, month), formatStr, {
        locale: this.dateService.locale,
      }),
    );
  }

  getYearName(date: string): string {
    return format(new Date(date), 'yyyy', {
      locale: this.dateService.locale,
    });
  }

  getFirstDayOfWeek(): number {
    return this.dateService.locale.options.weekStartsOn;
  }

  getNumDaysInMonth(date: string): number {
    return getDaysInMonth(new Date(date));
  }

  clone(date: string): string {
    return date;
  }

  createDate(year: number, month: number, date: number): string {
    return new Date(Date.UTC(year, month, date)).toISOString();
  }

  today(): string {
    var today = new Date();
    return new Date(Date.UTC(today.getFullYear(), today.getMonth(), today.getDate())).toISOString();
  }

  parse(value: any, parseFormat: any): string {
    if (value) {
      if (typeof value === 'string') {
        let date = parse(value, parseFormat, new Date(), {
          locale: this.dateService.locale,
        });

        if (isNaN(date.getTime())) {
          return null;
        }

        return date.toISOString();
      } else if (typeof value === 'number') {
        return toDate(value).toISOString();
      } else if (value instanceof Date) {
        return value.toISOString();
      }
    }
    return null;
  }

  format(date: string, displayFormat: any): string {
    return format(new Date(date), displayFormat, { locale: this.dateService.locale });
  }

  addCalendarYears(date: string, years: number): string {
    return addYears(new Date(date), years).toISOString();
  }

  addCalendarMonths(date: string, months: number): string {
    return addMonths(new Date(date), months).toISOString();
  }

  addCalendarDays(date: string, days: number): string {
    return addDays(new Date(date), days).toISOString();
  }

  toIso8601(date: string): string {
    return date;
  }

  isDateInstance(obj: any): boolean {
    return this.isValid(obj as string);
  }

  isValid(date: string): boolean {
    return ISO_8601_REGEX.test(date);
  }

  invalid(): string {
    return null;
  }

  deserialize(value: any): string | null {
    return value ? (this.isValid(value) ? new Date(value).toISOString() : this.invalid()) : null;
  }
}
