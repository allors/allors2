import { AfterViewInit, Component, ElementRef, ViewEncapsulation, Optional, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { RoleField } from 'src/allors/angular';

import Quill, { QuillOptionsStatic } from 'quill';
import TurndownService from 'turndown';
import * as marked from 'marked';

@Component({
  selector: 'a-mat-quill',
  template: `
<h4>{{label}}</h4>
<div>
  <div #quill></div>
</div>
  `,
  styles: [`
.ql-container .ql-editor {
}
  `],
  encapsulation: ViewEncapsulation.None
})
export class AllorsMaterialQuillComponent extends RoleField implements AfterViewInit {

  @ViewChild('quill', { static: true })
  quillDiv: ElementRef;

  quill: Quill;

  options: QuillOptionsStatic;

  private turndownService: TurndownService;

  constructor(
    @Optional() parentForm: NgForm,
  ) {
    super(parentForm);

    this.turndownService = new TurndownService();
  }

  ngAfterViewInit() {
    this.options = {
      modules: {
        toolbar: [
          [{ header: [1, 2, 3, 4, 5, 6, false] }, { header: 1 }, { header: 2 }],

          ['bold', 'italic', 'underline', 'strike'],

          ['blockquote'],

          [{ list: 'ordered' }, { list: 'bullet' }],
          [{ script: 'sub' }, { script: 'super' }],
          [{ indent: '-1' }, { indent: '+1' }],

          [{ color: new Array<any>() }, { background: new Array<any>() }],
          [{ align: new Array<any>() }],

          // ['link', 'image', 'video'],

          ['clean'],
        ]
      },
      placeholder: 'Insert text here ...',
      readOnly: false,
      theme: 'snow',
      scrollingContainer: document.body,
    };

    this.quill = new Quill(this.quillDiv.nativeElement, this.options);
    this.quill.enable(this.disabled || this.canWrite);
    this.quill.pasteHTML(this.html);

    this.quill.on('text-change', () => {
      this.html = this.quillDiv.nativeElement.children[0].innerHTML;
    });
  }

  get isMarkdown(): boolean {
    return this.roleType.mediaType === 'text/markdown';
  }

  get html(): string {

    if (this.isMarkdown) {
      return this.model ? marked.parser(marked.lexer(this.model)) : null;
    }

    return this.model;
  }

  set html(value: string) {
    if (value === '<p><br></p>') {
      value = null;
    }

    if (this.isMarkdown) {
      if (value !== undefined || value !== null) {
        value = this.turndownService.turndown(value);
      }
    }

    this.model = value;
  }
}
